using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MihaZupan;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using YogurtTheBot.Game.Server.RabbitMq;

namespace YogurtTheBot.Telegram.Polling
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
            System.Console.Title = botInfo.Username;

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
                    replyMarkup: new ReplyKeyboardMarkup(gameMessage
                        .Suggestions
                        ?.Select(s => new KeyboardButton(s.Text))
                        ?? new KeyboardButton[0]
                    ),
                    cancellationToken: cancellationTokenSource.Token
                );

                // ReSharper disable once AccessToDisposedClosure
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume("telegram", false, rabbitConsumer);

            System.Console.WriteLine($"Start listening for @{botInfo.Username}");
            System.Console.ReadLine();

            cancellationTokenSource.Cancel();
        }

        private static TelegramBotClient SetupBot(IConfigurationSection proxyConfiguration, string token)
        {
            TelegramBotClient bot;

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
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Confidentional.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}