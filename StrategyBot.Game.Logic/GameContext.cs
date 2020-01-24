using System;
using System.Threading.Tasks;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Data.Mongo;
using StrategyBot.Game.Logic.Entities;

namespace StrategyBot.Game.Logic
{
    public class GameContext
    {
        public IUnitOfWork UnitOfWork { get; }

        public GameContext(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task ProcessMessage(IncomingMessage message)
        {
            Console.WriteLine(message.Text);

            await Task.CompletedTask;
        }
    }
}