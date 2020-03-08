using System.Linq;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class LocalizedTerminal : Expression
    {
        private string _key;

        public LocalizedTerminal(string key)
        {
            _key = key;
        }
        
        public override ParsingResult? TryParse(ParsingContext parsingContext)
        {
            Localization localization = parsingContext.Localizer.GetString(_key, parsingContext.Locale);
            
            return new OneOf(localization.Values.Select(s => s.AsTerm())).TryParse(parsingContext);
        }
    }
}