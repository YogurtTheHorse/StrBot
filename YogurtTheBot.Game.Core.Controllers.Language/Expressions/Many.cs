using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class Many : Expression
    {
        private readonly Sequence _sequence;

        public override ParsingResult? TryParse(ParsingContext parsingContext) => _sequence.TryParse(parsingContext);

        public Many(Expression expression)
        {
            _sequence = new Sequence(expression, this | Empty);
        }
    }
}