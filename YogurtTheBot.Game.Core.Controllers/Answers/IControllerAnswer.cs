using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Answers
{
    public interface IControllerAnswer
    {
        string Text { get; }
        
        Suggestion[] Suggestions { get; }
    }
}