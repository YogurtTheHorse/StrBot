using System.Collections.Generic;
using System.Linq;
using YogurtTheBot.Game.Core.Controllers.Language.Expressions;

namespace YogurtTheBot.Game.Core.Controllers.Language.Nodes
{
    public class ListNode : INode
    {
        public int Start { get; }

        public int End { get; }

        public Expression Expression { get; }
        
        public string Value { get; }
        public IList<INode> Nodes { get; }

        public ListNode(Expression expression, IList<INode> nodes, string text)
        {
            Expression = expression;
            Nodes = nodes;
            Start = nodes.First().Start;
            End = nodes.Last().End;
            Value = text.Substring(Start, End - Start);
        }
    }
}