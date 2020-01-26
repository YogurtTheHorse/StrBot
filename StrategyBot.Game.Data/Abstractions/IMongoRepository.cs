using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace StrategyBot.Game.Data.Abstractions
{
    public interface IMongoRepository<T>
    {
        async Task<IEnumerable<T>> GetAll() => await GetAll(e => true);
        
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);

        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);

        Task Insert(T entity);

        Task InsertMany(IEnumerable<T> entities);

        Task Delete(T entity);

        Task DeleteMany(IEnumerable<T> entities);

        Task Update(T entity);

        async Task UpdateMany(IEnumerable<T> entities) => await Task.WhenAll(entities.Select(Update));
        
        Task<T> GetById(ObjectId id);
    }   
}