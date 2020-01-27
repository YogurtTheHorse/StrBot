using MongoDB.Bson;

namespace StrategyBot.Game.Interface.Entities
{
    public class IncomingMessage
    {
        public string Text { get; set; }
        
        public ObjectId PlayerId { get; set; }
    }
}