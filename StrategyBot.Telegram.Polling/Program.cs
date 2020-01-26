using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MihaZupan;
using RabbitMQ.Client;
using StrategyBot.Game.Logic.Entities;
using StrategyBot.Game.Server.RabbitMq;
using Telegram.Bot;

namespace StrategyBot.Telegram.Polling
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = BuildConfiguration();

            // IConfigurationSection proxyConfiguration = configuration.GetSection("Bot:Proxy");
            // string token = configuration.GetSection("Bot:Token").Value;
            //
            // TelegramBotClient bot = SetupBot(proxyConfiguration, token);
            //
            // User botInfo = await bot.GetMeAsync();
            // Console.Title = botInfo.Username;
            //
            // var cancellationTokenSource = new CancellationTokenSource();
            //
            // bot.StartReceiving(new UpdateHandler(gameContext, bot), cancellationTokenSource.Token);
            //
            // Console.WriteLine($"Start listening for @{botInfo.Username}");
            // Console.ReadLine();
            // 
            // cancellationTokenSource.Cancel();

            var rabbitMqSettings = configuration
                .GetSection(nameof(RabbitMqSettings))
                .Get<RabbitMqSettings>();

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.Hostname
            };


            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.SetupServerQueue(rabbitMqSettings);

            channel.BasicPublish(
                rabbitMqSettings.MessagesExchange,
                rabbitMqSettings.ServersQueue,
                null,
                new MessageFromSocialNetwork
                {
                    Text = "test",
                    PlayerSocialId = "test",
                    ReplyBackQueueName = "telegram"
                }.EncodeObject()
            );
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