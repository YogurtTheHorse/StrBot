using System;
using System.Threading.Tasks;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Logic.Entities;

namespace StrategyBot.Game.Logic
{
    public class GameContext
    {
        public IGameCommunicator GameCommunicator { get; set; }
        
        public IMongoUnitOfWork MongoUnitOfWork { get; }

        public GameContext(IMongoUnitOfWork mongoUnitOfWork, IGameCommunicator gameCommunicator)
        {
            GameCommunicator = gameCommunicator;
            MongoUnitOfWork = mongoUnitOfWork;
        }

        public async Task ProcessMessage(IncomingMessage message)
        {
            Console.WriteLine(message.Text);

            await Task.CompletedTask;
        }
    }
}