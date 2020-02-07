using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StrategyBot.Game.Core;
using StrategyBot.Game.Logic;
using StrategyBot.Game.Core.Communications;
using StrategyBot.Game.Core.Communications.Pipeline;
using StrategyBot.Game.Core.Controllers.Autofac;
using StrategyBot.Game.Core.Localizations;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Data.Mongo;
using StrategyBot.Game.Server.RabbitMq;
using StrategyBot.Game.Server.YamlLocalization;

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

            var localizationOptions = configuration
                .GetSection(nameof(LocalizationOptions))
                .Get<LocalizationOptions>();

            var factory = new ConnectionFactory
            {
                HostName = rabbitMqSettings.Hostname,
                DispatchConsumersAsync = true
            };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.SetupServerQueue(rabbitMqSettings);

            var iocContainerBuilder = new ContainerBuilder();

            iocContainerBuilder
                .RegisterInstance(mongoSettings);
            iocContainerBuilder
                .RegisterInstance(rabbitMqSettings);
            iocContainerBuilder
                .RegisterInstance(localizationOptions);

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
                .RegisterType<YamlLocalizer>()
                .As<ILocalizer>()
                .SingleInstance();

            iocContainerBuilder
                .RegisterGeneric(typeof(MongoRepository<>))
                .As(typeof(IMongoRepository<>))
                .SingleInstance();

            iocContainerBuilder
                .RegisterInstance(channel)
                .As<IModel>()
                .SingleInstance();

            iocContainerBuilder.RegisterControllers(typeof(MainMenuController).Assembly);

            iocContainerBuilder
                .Register(c => c
                    .Resolve<IEnumerable<IMiddleware>>()
                    .Aggregate(
                        new PipelineMessageProcessor(),
                        (p, m) => p.Use(m)
                    ))
                .As<IMessageProcessor>();

            iocContainerBuilder
                .RegisterInstance(new Random());

            iocContainerBuilder
                .RegisterType<Localization>()
                .AsSelf();

            IContainer container = iocContainerBuilder.Build();
            var m = container.Resolve<IMiddleware>();

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

                    var players = container.Resolve<IMongoRepository<PlayerState>>();
                    ObjectId? playerId = (await players.GetFirstOrDefault(p =>
                        p.SocialId == message.PlayerSocialId &&
                        p.ReplyQueueName == message.ReplyBackQueueName
                    ))?.Key;

                    var gameContext = container.Resolve<GameContext>();

                    if (playerId == null)
                    {
                        playerId = await gameContext.CreatePlayer(
                            message.PlayerSocialId,
                            message.ReplyBackQueueName
                        );
                    }

                    await gameContext.ProcessMessage(new IncomingMessage
                    {
                        Text = message.Text,
                        PlayerId = playerId.Value
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