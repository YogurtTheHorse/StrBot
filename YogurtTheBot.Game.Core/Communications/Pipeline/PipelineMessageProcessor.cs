using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YogurtTheBot.Game.Core.Communications.Pipeline
{
    public class PipelineMessageProcessor<T> : IMessageProcessor<T>
    {
        private List<IMiddleware<T>> _middlewares;

        public PipelineMessageProcessor()
        {
            _middlewares = new List<IMiddleware<T>>();
        }

        public PipelineMessageProcessor<T> Use(IMiddleware<T> middleware)
        {
            _middlewares.Add(middleware);

            return this;
        }

        private async Task ProcessMessage(IncomingMessage message, PlayerInfo info, T data, int i)
        {
            if (i >= _middlewares.Count) return;

            await _middlewares[i].Pipe(
                message,
                info,
                data,
                async () => { await ProcessMessage(message, info, data, i + 1); }
            );
        }

        public async Task ProcessMessage(IncomingMessage message, PlayerInfo info, T data)
        {
            if (_middlewares.Any())
            {
                await ProcessMessage(message, info, data, 0);
            }
        }
    }
}