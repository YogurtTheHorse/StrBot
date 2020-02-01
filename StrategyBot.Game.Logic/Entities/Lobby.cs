using MongoDB.Bson;
using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Entities
{
    public class Lobby : MongoModel
    {
        public int Level { get; set; }
        
        public ObjectId[] PlayersIds { get; set; }
    }
}