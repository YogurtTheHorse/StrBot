using MongoDB.Bson;

namespace StrategyBot.Game.Interface
{
    public class GameAnswer
    {
        public ObjectId PlayerId { get; set; }
        
        public string Text { get; set; }
    }
}