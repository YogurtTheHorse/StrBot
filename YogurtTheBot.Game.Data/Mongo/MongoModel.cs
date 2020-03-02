using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YogurtTheBot.Game.Data.Mongo
{
    public abstract class MongoModel
    {
        [BsonId]
        public ObjectId Key { get; set; }
    }
}