using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using StrategyBot.Game.Data.Abstractions;

namespace StrategyBot.Game.Data.Mongo
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoUnitOfWork(MongoSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.Database);

            if (!settings.EnumAsString) return;
            
            var pack = new ConventionPack {
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("EnumConventionAsString", pack, t => true);
        }

        public IRepository<T> GetRepository<T>() where T : MongoModel => new MongoRepository<T>(_database);
    }
}