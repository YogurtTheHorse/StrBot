using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Data.Mongo;
using StrategyBot.Game.Logic;
using StrategyBot.Game.Logic.Entities;
using StrategyBot.Game.Logic.Models;
using StrategyBot.Game.Server.RabbitMq;

namespace StrategyBot.Game.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = BuildConfiguration();
            var iocContainerBuilder = new ContainerBuilder();

            iocContainerBuilder
                .RegisterInstance(
                    configuration
                        .GetSection(nameof(MongoSettings))
                        .Get<MongoSettings>()
                )
                .As<MongoSettings>();
            iocContainerBuilder
                .RegisterType<MongoUnitOfWork>()
                .As<IMongoUnitOfWork>()
                .SingleInstance();
            iocContainerBuilder
                .RegisterType<GameContext>()
                .SingleInstance();
            iocContainerBuilder
                .RegisterType<RabbitMqCommunicator>()
                .As<IGameCommunicator>()
                .SingleInstance();

            var rabbitMqSettings = configuration
                .GetSection(nameof(RabbitMqSettings))
                .Get<RabbitMqSettings>();

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.Hostname,
                DispatchConsumersAsync = true
            };


            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();
            iocContainerBuilder
                .RegisterInstance(channel)
                .As<IModel>()
                .SingleInstance();

            channel.SetupServerQueue(rabbitMqSettings);

            IContainer container = iocContainerBuilder.Build();
            using ILifetimeScope scope = container.BeginLifetimeScope();

            var messagesConsumer = new AsyncEventingBasicConsumer(channel);
            messagesConsumer.Received += async (model, ea) =>
            {
                var message = ea.Body.DecodeObject<MessageFromSocialNetwork>();

                using ILifetimeScope scope = container.BeginLifetimeScope();
                IMongoRepository<Player> players = scope.Resolve<IMongoUnitOfWork>().GetRepository<Player>();
                Player player = await players.GetFirstOrDefault(p =>
                                    p.SocialId == message.PlayerSocialId &&
                                    p.ReplyQueueName == message.ReplyBackQueueName
                                )
                                ?? new Player
                                {
                                    Key = ObjectId.GenerateNewId(),
                                    SocialId = message.PlayerSocialId,
                                    ReplyQueueName = message.ReplyBackQueueName
                                };

                var gameContext = scope.Resolve<GameContext>();
                await gameContext.ProcessMessage(new IncomingMessage
                {
                    Text = message.Text,
                    PlayerId = player.Key
                });
            };

            channel.BasicConsume(
                rabbitMqSettings.ServersQueue,
                autoAck: true,
                consumer: messagesConsumer
            );

            Console.WriteLine("Listening...");
            Console.ReadLine();
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}