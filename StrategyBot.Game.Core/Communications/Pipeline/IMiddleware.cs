using System;
using System.Threading.Tasks;

namespace StrategyBot.Game.Core.Communications.Pipeline
{
    public interface IMiddleware
    {
        Task Pipe(IncomingMessage message, PlayerState state, PlayerData data, Func<Task> next);
    }
}