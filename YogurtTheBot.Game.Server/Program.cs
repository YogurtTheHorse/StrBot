using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.FSharp;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using YogurtTheBot.Game.Logic;
using YogurtTheBot.Game.Core.Controllers.Autofac;
using YogurtTheBot.Game.Core;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Communications.Pipeline;
using YogurtTheBot.Game.Core.Controllers;
using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data.Abstractions;
using YogurtTheBot.Game.Data.Mongo;
using YogurtTheBot.Game.Server.RabbitMq;
using YogurtTheBot.Game.Server.YamlLocalization;
using Localization = YogurtTheBot.Game.Core.Localizations.Localization;

namespace YogurtTheBot.Game.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            FSharpSerializer.Register();
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
                UserName = rabbitMqSettings.Username,
                Password = rabbitMqSettings.Password,
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
                .RegisterType<GameContext<PlayerData>>()
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

            iocContainerBuilder.RegisterControllers<PlayerData>(typeof(MainMenuController).Assembly);
            iocContainerBuilder
                .RegisterType<ControllerMiddleware<PlayerData>>()
                .AsSelf();

            iocContainerBuilder
                .Register(c =>
                    new PipelineMessageProcessor<PlayerData>()
                        .Use(c.Resolve<ControllerMiddleware<PlayerData>>())
                )
                .As<IMessageProcessor<PlayerData>>();

            iocContainerBuilder
                .RegisterInstance(new Random());

            iocContainerBuilder
                .RegisterType<Localization>()
                .AsSelf();

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

                    var players = container.Resolve<IMongoRepository<PlayerInfo>>();
                    ObjectId? playerId = (await players.GetFirstOrDefault(p =>
                        p.SocialId == message.PlayerSocialId &&
                        p.ReplyQueueName == message.ReplyBackQueueName
                    ))?.Key;

                    var gameContext = container.Resolve<GameContext<PlayerData>>();

                    if (playerId == null)
                    {
                        playerId = await gameContext.CreatePlayer(
                            message.PlayerSocialId,
                            message.ReplyBackQueueName,
                            message.Locale
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
                    Console.Error.WriteLine(e.ToString());
                }
            };

        private static IConfigurationRoot BuildConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables("GAME_");

            return builder.Build();
        }
    }
}