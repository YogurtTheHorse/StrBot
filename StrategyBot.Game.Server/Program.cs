using System;
using System.IO;
using System.Reflection;
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
using StrategyBot.Game.Logic.Screens;
using StrategyBot.Game.Screens;
using StrategyBot.Game.Server.RabbitMq;

namespace StrategyBot.Game.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = BuildConfiguration();

            var rabbitMqSettings = configuration
                .GetSection(nameof(RabbitMqSettings))
                .Get<RabbitMqSettings>();
            var mongoSettings = configuration
                .GetSection(nameof(MongoSettings))
                .Get<MongoSettings>();

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.Hostname,
                DispatchConsumersAsync = true
            };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.SetupServerQueue(rabbitMqSettings);
            
            var iocContainerBuilder = new ContainerBuilder();

            iocContainerBuilder
                .RegisterInstance(mongoSettings)
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

            iocContainerBuilder
                .RegisterInstance(rabbitMqSettings);
            
            iocContainerBuilder
                .RegisterInstance(channel)
                .As<IModel>()
                .SingleInstance();

            iocContainerBuilder
                .RegisterAssemblyTypes(typeof(MainMenuScreen).Assembly)
                .Where(t => typeof(IScreen).IsAssignableFrom(t))
                .As<IScreen>()
                .PreserveExistingDefaults();

            iocContainerBuilder
                .RegisterType<StackScreenController>()
                .As<IScreenController>();

            IContainer container = iocContainerBuilder.Build();

            var messagesConsumer = new AsyncEventingBasicConsumer(channel);
            messagesConsumer.Received += MessagesConsumerOnReceived(container);

            channel.BasicConsume(
                rabbitMqSettings.ServersQueue,
                autoAck: true,
                consumer: messagesConsumer
            );
            

            Console.WriteLine("Listening...");
            Console.ReadLine();
        }

        private static AsyncEventHandler<BasicDeliverEventArgs> MessagesConsumerOnReceived(IContainer container) =>
            async (model, ea) =>
            {
                try
                {
                    var message = ea.Body.DecodeObject<MessageFromSocialNetwork>();

                    IMongoRepository<PlayerInfo> players = container.Resolve<IMongoUnitOfWork>().GetRepository<PlayerInfo>();
                    PlayerInfo playerInfo = await players.GetFirstOrDefault(p =>
                        p.SocialId == message.PlayerSocialId &&
                        p.ReplyQueueName == message.ReplyBackQueueName
                    );
                    
                    if (playerInfo == null)
                    {
                        playerInfo = new PlayerInfo
                        {
                            Key = ObjectId.GenerateNewId(),
                            SocialId = message.PlayerSocialId,
                            ReplyQueueName = message.ReplyBackQueueName
                        };
                        await players.Insert(playerInfo);
                    }

                    var gameContext = container.Resolve<GameContext>();
                    await gameContext.ProcessMessage(new IncomingMessage
                    {
                        Text = message.Text,
                        PlayerId = playerInfo.Key
                    });
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    Console.Error.WriteLine(e.StackTrace);
                }
            };

        private static IConfigurationRoot BuildConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}