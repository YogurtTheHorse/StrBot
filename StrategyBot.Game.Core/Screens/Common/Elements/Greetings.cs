using System.Threading.Tasks;
using StrategyBot.Game.Data;
using StrategyBot.Game.Logic.Communications;
using StrategyBot.Game.Logic.Localizations;

namespace StrategyBot.Game.Logic.Screens.Common.Elements
{
    public class Greetings : CommonScreenElement
    {
        private readonly LocalizationDescription _description;
        private readonly ILocalizer _localizer;
        private readonly IGameCommunicator _gameCommunicator;

        public Greetings(LocalizationDescription description, ILocalizer localizer, IGameCommunicator gameCommunicator)
        {
            _description = description;
            _localizer = localizer;
            _gameCommunicator = gameCommunicator;
        }
        
        public override async Task<bool> OnOpen(PlayerState state, PlayerData data, CommonScreen screen)
        {
            await _gameCommunicator.Answer(new GameAnswer
            {
                Text = _localizer.GetString(
                    _description.Key,
                    state.Locale
                ).Format(_description.ArgsFactory(state, data)).Value,
                PlayerId = data.Key,
                Suggestions = screen.GetSuggestions(state, data)
            });

            return false;
        }
    }
}