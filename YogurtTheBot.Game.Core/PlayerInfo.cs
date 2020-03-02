using YogurtTheBot.Game.Data.Mongo;

namespace YogurtTheBot.Game.Core
{
    public class PlayerInfo : MongoModel
    {
        public string ReplyQueueName { get; set; }

        public string SocialId { get; set; }
        
        public string Locale { get; set; }
    }
}