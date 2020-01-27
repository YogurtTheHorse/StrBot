using System.Threading.Tasks;
using StrategyBot.Game.Logic;
using StrategyBot.Game.Logic.Entities;
using StrategyBot.Game.Logic.Models;
using StrategyBot.Game.Logic.Screens;

namespace StrategyBot.Game.Screens
{
    [MainScreen]
    public class MainMenuScreen : IScreen
    {
        private readonly IGameCommunicator _gameCommunicator;

        public MainMenuScreen(IGameCommunicator gameCommunicator)
        {
            _gameCommunicator = gameCommunicator;
        }
        
        public async Task ProcessMessage(IncomingMessage message, PlayerData playerData)
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