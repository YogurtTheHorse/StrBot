using MongoDB.Bson;

namespace StrategyBot.Game.Data
{
    public class GameAnswer
    {
        public ObjectId PlayerId { get; set; }
        
        public string Text { get; set; }
        
        public Suggestion[] Suggestions { get; set; }
    }
}