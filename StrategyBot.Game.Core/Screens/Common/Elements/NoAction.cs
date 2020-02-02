using System.Threading.Tasks;
using StrategyBot.Game.Data;
using StrategyBot.Game.Logic.Communications;
using StrategyBot.Game.Logic.Localizations;

namespace StrategyBot.Game.Logic.Screens.Common.Elements
{
    public class NoAction : CommonScreenElement
    {
        private readonly string _greetKey;
        private readonly ILocalizer _localizer;
        private readonly IGameCommunicator _gameCommunicator;

        public NoAction(string greetKey, ILocalizer localizer, IGameCommunicator gameCommunicator)
        {
            _greetKey = greetKey;
            _localizer = localizer;
            _gameCommunicator = gameCommunicator;
        }

        public override async Task<bool> OnOpen(PlayerState state, PlayerData data, CommonScreen screen) =>
            await SendAnswer(state, data, screen);

        public override async Task<bool> ProcessMessage(
            IncomingMessage msg,
            PlayerState state,
            PlayerData data,
            CommonScreen screen
        ) => await SendAnswer(state, data, screen);

        private async Task<bool> SendAnswer(PlayerState state, PlayerData data, CommonScreen screen)
        {
            await _gameCommunicator.Answer(new GameAnswer
            {
                Text = _localizer.GetString(
                    _greetKey,
                    state.Locale
                ).Value,
                PlayerId = state.Key,
                Suggestions = screen.GetSuggestions(state, data)
            });

            return false;
        }
    }
}