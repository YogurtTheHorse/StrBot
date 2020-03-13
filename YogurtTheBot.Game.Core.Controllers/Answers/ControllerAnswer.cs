using System;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Answers
{
    public class ControllerAnswer : IControllerAnswer
    {
        public string Text { get; set; }
        public Suggestion[] Suggestions { get; set; }
    }
}