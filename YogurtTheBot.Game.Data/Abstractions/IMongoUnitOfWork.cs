using MongoDB.Driver;
using YogurtTheBot.Game.Data.Mongo;

namespace YogurtTheBot.Game.Data.Abstractions
{
    public interface IMongoUnitOfWork
    {
        IMongoRepository<T> GetRepository<T>() where T : MongoModel;
        
        IMongoDatabase Database { get; }
    }
}