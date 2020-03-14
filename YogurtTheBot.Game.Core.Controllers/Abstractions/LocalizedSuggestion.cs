using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public class LocalizedSuggestion : IControllerSuggestion
    {
        private readonly string _path;

        public LocalizedSuggestion(string path)
        {
            _path = path;
        }

        public Suggestion GetSuggestion(ILocalizer localizer, string locale) =>
            localizer.GetString(_path, locale).Value;
    }
}