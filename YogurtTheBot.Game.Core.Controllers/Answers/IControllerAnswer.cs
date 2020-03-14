using YogurtTheBot.Game.Core.Controllers.Abstractions;

namespace YogurtTheBot.Game.Core.Controllers.Answers
{
    public interface IControllerAnswer
    {
        string Text { get; }
        
        IControllerSuggestion[] Suggestions { get; }
    }
}