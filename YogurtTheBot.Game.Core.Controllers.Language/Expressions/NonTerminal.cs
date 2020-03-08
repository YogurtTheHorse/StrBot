using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class NonTerminal : Expression
    {
        public string Name { get; set; }
        public Expression Rule { get; set; }

        public NonTerminal(string name)
        {
            Name = name;
        }
        
        public override ParsingResult? TryParse(ParsingContext parsingContext)
        {
            ParsingResult? result = Rule.TryParse(parsingContext);

            if (result is null) return null;

            return new ParsingResult
            {
                Possibilities = result.Possibilities
            };
        }
    }
}