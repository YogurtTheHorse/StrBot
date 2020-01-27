using System;
using System.Threading.Tasks;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Logic.Entities;

namespace StrategyBot.Game.Logic
{
    public class GameContext
    {
        private readonly IGameCommunicator _gameCommunicator;

        private readonly IMongoUnitOfWork _unitOfWork;

        public GameContext(IMongoUnitOfWork unitOfWork, IGameCommunicator gameCommunicator)
        {
            _gameCommunicator = gameCommunicator;
            _unitOfWork = unitOfWork;
        }

        public async Task ProcessMessage(IncomingMessage message)
        {
            Console.WriteLine(message.Text);

            await _gameCommunicator.Answer(new GameAnswer
            {
                PlayerId = message.PlayerId,
                Text = "I've got " + message.Text
            });
        }
    }
}