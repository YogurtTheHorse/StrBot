using StrategyBot.Game.Data;

namespace StrategyBot.Game.Core.Controllers.Answers
{
    public class ControllerAnswer : IControllerAnswer
    {
        public string Text { get; set; }
        public Suggestion[] Suggestions { get; set; }
    }
}