using System.Threading.Tasks;
using StrategyBot.Game.Interface;
using StrategyBot.Game.Interface.Entities;
using StrategyBot.Game.Interface.Models;
using StrategyBot.Game.Interface.Screens;

namespace StrategyBot.Game.Logic.Screens
{
    [MainScreen]
    public class MainMenuScreen : IScreen
    {
        private readonly IGameCommunicator _gameCommunicator;

        public MainMenuScreen(IGameCommunicator gameCommunicator)
        {
            _gameCommunicator = gameCommunicator;
        }
        
        public async Task ProcessMessage(IncomingMessage message, PlayerState playerData)
        {
            await _gameCommunicator.Answer(new GameAnswer
            {
                Text = "Hi, you are in main menu",
                PlayerId = message.PlayerId
            });

            await Task.CompletedTask;
        }
    }
}