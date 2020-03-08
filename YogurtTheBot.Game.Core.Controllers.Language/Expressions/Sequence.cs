using System.Collections.Generic;
using System.Linq;
using YogurtTheBot.Game.Core.Controllers.Language.Nodes;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class Sequence : Expression
    {
        private readonly Expression _e1, _e2;

        public Sequence(Expression e1, Expression e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        public override ParsingResult? TryParse(ParsingContext parsingContext)
        {
            ParsingResult? result = _e1.TryParse(parsingContext);

            if (result is null) return null;
            
            var possibilities = new List<Possibility>();

            foreach (Possibility possibility in result.Possibilities)
            {
                ParsingResult? secondResult = _e2.TryParse(possibility.Context);
                
                if (secondResult is null) continue;
                
                possibilities.AddRange(secondResult
                    .Possibilities
                    .Select(p => 
                        new Possibility
                        {
                            Node = new ListNode(this, new []{possibility.Node, p.Node}),
                            Context = p.Context
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