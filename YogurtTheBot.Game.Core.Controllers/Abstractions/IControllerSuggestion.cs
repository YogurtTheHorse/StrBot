using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public interface IControllerSuggestion
    {
        Suggestion GetSuggestion(ILocalizer localizer, string locale);
    }
}