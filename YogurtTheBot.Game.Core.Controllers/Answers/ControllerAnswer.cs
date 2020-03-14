using YogurtTheBot.Game.Core.Controllers.Abstractions;

namespace YogurtTheBot.Game.Core.Controllers.Answers
{
    public class ControllerAnswer : IControllerAnswer
    {
        public string Text { get; set; }
        public IControllerSuggestion[] Suggestions { get; set; }
    }
}