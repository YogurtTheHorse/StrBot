using System.Collections.Generic;

namespace StrategyBot.Game.Logic.Models
{
    public class PlayerData
    {
        public Dictionary<string, object> TemporaryVariables { get; set; } = new Dictionary<string, object>();

        public Stack<string> ScreensStack = new Stack<string>();
    }
}