using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Logic.Models
{
    public class PlayerInfo : MongoModel
    {
        public string ReplyQueueName { get; set; }
        
        public string SocialId { get; set; }
    }
}