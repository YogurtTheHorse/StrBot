using MongoDB.Bson;

namespace StrategyBot.Game.Core
{
    public class GameAnswer
    {
        public ObjectId PlayerId { get; set; }
        
        public string Text { get; set; }
        
        public string[] Suggestions { get; set; }
    }
}