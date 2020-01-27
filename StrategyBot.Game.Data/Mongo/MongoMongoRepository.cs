using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using StrategyBot.Game.Data.Abstractions;

namespace StrategyBot.Game.Data.Mongo
{
    public class MongoMongoRepository<T> : IMongoRepository<T> where T : MongoModel
    {
        private readonly IMongoCollection<T> _collection;

        public MongoMongoRepository(IMongoDatabase database)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(map =>
                {
                    map.AutoMap();
                    map.SetIdMember(map.GetMemberMap(m => m.Key));
                });
            }

            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter) =>
            await (await _collection.FindAsync(filter)).ToListAsync();

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter) =>
            await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();

        public async Task Insert(T entity) => await _collection.InsertOneAsync(entity);

        public async Task InsertMany(IEnumerable<T> entities) => await _collection.InsertManyAsync(entities);

        public async Task Delete(T entity) =>
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq(m => m.Key, entity.Key));

        public async Task DeleteMany(IEnumerable<T> entities) =>
            await _collection.DeleteManyAsync(Builders<T>
                .Filter
                .In(m => m.Key, entities.Select(e => e.Key))
            );

        public async Task Update(T entity) =>
            await _collection.FindOneAndReplaceAsync(
                Builders<T>.Filter.Eq(m => m.Key, entity.Key),
                entity
            );

        public async Task<T> GetById(ObjectId id) =>
            await (
                await _collection.FindAsync(Builders<T>.Filter.Eq(m => m.Key, id))
            ).FirstOrDefaultAsync();
    }
}