using StrategyBot.Game.Data.Mongo;

namespace StrategyBot.Game.Data.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : MongoModel;
    }
}