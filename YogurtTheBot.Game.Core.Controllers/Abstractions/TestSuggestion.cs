using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public class TestSuggestion : IControllerSuggestion
    {
        private readonly string _text;

        public TestSuggestion(string text)
        {
            _text = text;
        }

        public Suggestion GetSuggestion(ILocalizer localizer, string locale) => _text;
    }
}