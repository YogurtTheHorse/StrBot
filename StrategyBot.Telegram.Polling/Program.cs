using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MihaZupan;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Data.Mongo;
using StrategyBot.Game.Logic;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StrategyBot.Telegram.Polling
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            IUnitOfWork unitOfWork = new MongoUnitOfWork(new MongoSettings
            {
                Database = "sb",
                ConnectionString = "mongodb://localhost:27017",
                EnumAsString = true
            });
            var gameContext = new GameContext(unitOfWork);

            IConfigurationRoot configuration = BuildConfiguration();

            IConfigurationSection proxyConfiguration = configuration.GetSection("Bot:Proxy");
            string token = configuration.GetSection("Bot:Token").Value;

            TelegramBotClient bot = SetupBot(proxyConfiguration, token);

            User botInfo = await bot.GetMeAsync();
            Console.Title = botInfo.Username;

            var cancellationTokenSource = new CancellationTokenSource();
            
            bot.StartReceiving(new UpdateHandler(gameContext, bot), cancellationTokenSource.Token);

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