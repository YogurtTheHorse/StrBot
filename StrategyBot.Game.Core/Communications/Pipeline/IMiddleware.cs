using System;
using System.Threading.Tasks;

namespace StrategyBot.Game.Logic.Communications.Pipeline
{
    public interface IMiddleware
    {
        Task Pipe(IncomingMessage message, PlayerState state, PlayerData data, Func<Task> next);
    }
}