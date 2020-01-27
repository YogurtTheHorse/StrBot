using System.Collections.Generic;
using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Interface.Models
{
    public class PlayerState : MongoModel
    {
        public string ReplyQueueName { get; set; }

        public string SocialId { get; set; }

        public Stack<string> ScreensStack { get; set; } = new Stack<string>();
    }
}