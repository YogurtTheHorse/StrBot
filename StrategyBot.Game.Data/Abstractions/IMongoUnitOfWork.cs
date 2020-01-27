using MongoDB.Driver;
using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Data.Abstractions
{
    public interface IMongoUnitOfWork
    {
        IMongoRepository<T> GetRepository<T>() where T : MongoModel;
        
        IMongoDatabase Database { get; }
    }
}