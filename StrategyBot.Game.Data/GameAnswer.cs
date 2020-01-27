using MongoDB.Bson;

namespace StrategyBot.Game.Logic
{
    public class GameAnswer
    {
        public ObjectId PlayerId { get; set; }
        
        public string Text { get; set; }
    }
}