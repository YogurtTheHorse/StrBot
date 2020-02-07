using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrategyBot.Game.Core.Communications.Pipeline
{
    public class PipelineMessageProcessor : IMessageProcessor
    {
        private List<IMiddleware> _middlewares;

        public PipelineMessageProcessor()
        {
            _middlewares = new List<IMiddleware>();
        }

        public PipelineMessageProcessor Use(IMiddleware middleware)
        {
            _middlewares.Add(middleware);

            return this;
        }

        private async Task ProcessMessage(IncomingMessage message, PlayerState state, PlayerData data, int i)
        {
            if (i >= _middlewares.Count) return;

            await _middlewares[i].Pipe(
                message,
                state,
                data,
                async () => { await ProcessMessage(message, state, data, i + 1); }
            );
        }

        public async Task ProcessMessage(IncomingMessage message, PlayerState state, PlayerData data)
        {
            if (_middlewares.Any())
            {
                await ProcessMessage(message, state, data, 0);
            }
        }
    }
}