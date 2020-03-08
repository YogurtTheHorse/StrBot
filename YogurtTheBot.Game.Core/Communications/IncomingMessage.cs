using MongoDB.Bson;

namespace YogurtTheBot.Game.Core.Communications
{
    public class IncomingMessage
    {
        public string Text { get; set; }

        public ObjectId PlayerId { get; set; }
    }
}