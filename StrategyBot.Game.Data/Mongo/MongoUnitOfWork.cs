using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using StrategyBot.Game.Data.Abstractions;

namespace StrategyBot.Game.Data.Mongo
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private readonly MongoClient _client;

        public MongoUnitOfWork(MongoSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            Database = _client.GetDatabase(settings.Database);

            // ReSharper disable once InvertIf
            if (settings.EnumAsString)
            {
                var pack = new ConventionPack
                {
                    new EnumRepresentationConvention(BsonType.String)
                };

                ConventionRegistry.Register("EnumConventionAsString", pack, t => true);
            }
        }

        public IMongoRepository<T> GetRepository<T>() where T : MongoModel => new MongoRepository<T>(this);

        public IMongoDatabase Database { get; }
    }
}