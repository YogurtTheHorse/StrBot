using System.Threading.Tasks;
using StrategyBot.Game.Data.Abstractions;
using StrategyBot.Game.Entities;
using StrategyBot.Game.Interface;
using StrategyBot.Game.Interface.Entities;
using StrategyBot.Game.Interface.Models;
using StrategyBot.Game.Interface.Screens;

namespace StrategyBot.Game.Logic.Screens
{
    [MainScreen]
    public class MainMenuScreen : IScreen
    {
        private readonly IGamePCommunicator _gameCommunicator;
        private readonly ILocalizer _localizer;

        public MainMenuScreen(IGameCommunicator gameCommunicator, ILocalizer localizer)
        {
            _gameCommunicator = gameCommunicator;
            _localizer = localizer;
        }

        public async Task ProcessMessage(IncomingMessage message, PlayerState playerState, PlayerData playerData)
        {
            await _gameCommunicator.Answer(new GameAnswer
            {
                Text = "Hi, you are in main menu",
                PlayerId = message.PlayerId
            });

            await Task.CompletedTask;
        }

        public async Task OnOpen(PlayerState playerState, PlayerData playerData)
        {
            await _gameCommunicator.Answer(new GameAnswer
            {
                Text = _localizer.GetString(
                    "screens.main_menu.open_phrase",
                    playerState.Locale,
                    playerData.AttackShips,
                    playerData.DefenceShips
                ),
                PlayerId = playerData.Key,
                Suggestions = new[]
                {
                    _localizer.GetString("screens.main_menu.attack", playerState.Locale),
                    _localizer.GetString("screens.main_menu.select_skills", playerState.Locale),
                }
            });
        }
    }
}