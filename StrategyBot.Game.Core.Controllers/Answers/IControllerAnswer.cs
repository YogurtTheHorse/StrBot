using StrategyBot.Game.Data;

namespace StrategyBot.Game.Core.Controllers.Answers
{
    public interface IControllerAnswer
    {
        string Text { get; }
        
        Suggestion[] Suggestions { get; }
    }
}