using System.Collections.Generic;
using YogurtTheBot.Game.Core.Controllers.Language.Nodes;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class Terminal : Expression
    {
        public Terminal(string term)
        {
            Value = term;
        }

        public override ParsingResult? TryParse(ParsingContext ctx)
        {
            if (!ctx.ContinuesWith(Value)) return null;


            return new ParsingResult
            {
                Possibilities = new List<Possibility>
                {
                    new Possibility
                    {
                        Context = ctx.AppendTerminal(this),
                        Node = new SingleNode(this, ctx)
                    }
                }
            };
        }

        public string Value { get; }

        public static implicit operator Terminal(string term) => new Terminal(term);
    }
}