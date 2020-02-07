using System.Collections.Generic;
using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Core
{
    public class PlayerInfo : MongoModel
    {
        public string ReplyQueueName { get; set; }

        public string SocialId { get; set; }
        
        public string Locale { get; set; }
    }
}