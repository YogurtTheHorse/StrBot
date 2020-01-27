﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MihaZupan;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StrategyBot.Game.Logic;
using StrategyBot.Game.Server.RabbitMq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrategyBot.Telegram.Polling
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationRoot configuration = BuildConfiguration();

            IConfigurationSection proxyConfiguration = configuration.GetSection("Bot:Proxy");
            string token = configuration.GetSection("Bot:Token").Value;

            TelegramBotClient bot = SetupBot(proxyConfiguration, token);

            User botInfo = await bot.GetMeAsync();
            Console.Title = botInfo.Username;

            var cancellationTokenSource = new CancellationTokenSource();

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

            channel.SetupServerQueue(rabbitMqSettings);

            channel.QueueDeclare("telegram", true, false, false);
            channel.QueueBind(
                "telegram",
                rabbitMqSettings.MessagesExchange,
                new MessagesRoutingKeyBuilder()
                    .WithSocialNetwork("telegram")
                    .Build()
            );

            bot.StartReceiving(new UpdateHandler(channel, rabbitMqSettings), cancellationTokenSource.Token);

            var rabbitConsumer = new AsyncEventingBasicConsumer(channel);
            rabbitConsumer.Received += async (_, ea) =>
            {
                var gameMessage = ea.Body.DecodeObject<MessageToSocialNetwork>();

                await bot.SendTextMessageAsync(
                    gameMessage.PlayerSocialId,
                    gameMessage.Text,
                    cancellationToken: cancellationTokenSource.Token
                );
            };
            channel.BasicConsume("telegram", false, rabbitConsumer);

            Console.WriteLine($"Start listening for @{botInfo.Username}");
            Console.ReadLine();

            cancellationTokenSource.Cancel();
        }

        private static TelegramBotClient SetupBot(IConfigurationSection proxyConfiguration, string token)
        {
            TelegramBotClient bot = null;

            if (proxyConfiguration.Exists())
            {
                var proxy = new HttpToSocks5Proxy(
                    proxyConfiguration["Host"],
                    int.Parse(proxyConfiguration["Port"]),
                    proxyConfiguration["User"],
                    proxyConfiguration["Password"]
                )
                {
                    ResolveHostnamesLocally = true
                };

                bot = new TelegramBotClient(token, proxy);
            }
            else
            {
                bot = new TelegramBotClient(token);
            }

            return bot;
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}