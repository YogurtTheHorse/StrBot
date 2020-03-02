using System;
using System.Threading.Tasks;

namespace YogurtTheBot.Game.Core.Communications.Pipeline
{
    public interface IMiddleware<in T>
    {
        Task Pipe(IncomingMessage message, PlayerInfo info, T data, Func<Task> next);
    }
}