using System.Collections.Generic;
using System.Linq;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class OneOf : NonTerminal
    {
        private readonly IEnumerable<Expression> _expressions;

        public OneOf(IEnumerable<Expression> expressions)
        {
            _expressions = expressions;
        }

        public override ParsingResult? TryParse(ParsingContext parsingContext)
        {
            var possibilities = new List<Possibility>();

            foreach (Expression expression in _expressions)
            {
                ParsingResult? result = expression.TryParse(parsingContext);

                if (result is null) continue;

                possibilities.AddRange(result
                    .Possibilities
                    .Select(
                        p => new Possibility
                        {
                            Context = p.Context,
                            Node = p.Node
                        }
                    )
                );
            }

            if (possibilities.Count == 0) return null;

            return new ParsingResult
            {
                Possibilities = possibilities
            };
        }
    }
}