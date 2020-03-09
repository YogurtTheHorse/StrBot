using System.Linq;
using YogurtTheBot.Game.Core.Controllers.Language.Nodes;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class NonTerminal : Expression
    {
        public string Name { get; set; }
        public Expression Rule { get; set; }

        public NonTerminal(string name, Expression rule)
        {
            Name = name;
            Rule = rule;
        }

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
                Possibilities = result
                    .Possibilities
                    .Select(p => new Possibility
                    {
                        Context = p.Context,
                        Node = p.Node switch
                        {
                            SingleNode singleNode => new SingleNode(singleNode.Terminal, p.Context, this),
                            ListNode listNode => new ListNode(this, listNode.Nodes, p.Context.Text),
                            _ => p.Node
                        }
                    })
                    .ToArray()
            };
        }
    }
}