using YogurtTheBot.Game.Core.Controllers.Language.Expressions;

namespace YogurtTheBot.Game.Core.Controllers.Language.Nodes
{
    public interface INode
    {
        int Start { get; }
        
        int End { get; }
        
        Expression Expression { get; }
        
        string Value { get; }

        string ToString() => $"[{Start}:{End}] {Value}";
    }
}