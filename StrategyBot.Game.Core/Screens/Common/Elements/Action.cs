using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyBot.Game.Data;
using StrategyBot.Game.Logic.Communications;
using StrategyBot.Game.Logic.Localizations;

namespace StrategyBot.Game.Logic.Screens.Common.Elements
{
    public class Action : CommonScreenElement
    {
        private readonly LocalizationDescription _localizationDescription;
        private readonly Func<Task<bool>> _callback;
        private readonly ILocalizer _localizer;

        public Action(LocalizationDescription localizationDescription, Func<Task<bool>> callback, ILocalizer localizer)
        {
            _localizationDescription = localizationDescription;
            _callback = callback;
            _localizer = localizer;
        }

        public override async Task<bool> ProcessMessage(IncomingMessage message, PlayerState state, PlayerData data, CommonScreen screen)
        {
            Localization localization = _localizer.GetString(_localizationDescription.Key, state.Locale);

            if (localization.MatchesMessage(message, _localizationDescription.ArgsFactory(state, data)))
            {
                return await _callback();
            }

            return false;
        }

        public override IEnumerable<Suggestion> GetSuggestions(PlayerState state, PlayerData data)
        {
            return new Suggestion[]
            {
                _localizer.GetString(_localizationDescription.Key, state.Locale).Value
            };
        }
    }
}