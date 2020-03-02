using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using YogurtTheBot.Game.Data.Abstractions;

namespace YogurtTheBot.Game.Data.Mongo
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        public MongoUnitOfWork(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.Database);

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