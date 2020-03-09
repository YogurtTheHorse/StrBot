using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Nodes
{
    public class SingleNode : INode
    {
        public SingleNode(Terminal terminal, ParsingContext parsingContext)
        {
            Expression = terminal;
            Start = parsingContext.Position;
            End = Start + terminal.Value.Length;
            Value = terminal.Value;
        }

        public int Start { get; }
        
        public int End { get; }
        public Expression Expression { get; }
        
        public string Value { get; }
    }
}