using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Communications.Pipeline;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Answers;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers
{
    public class ControllerMiddleware<T> : IMiddleware<T> where T : IControllersData
    {
        private readonly IControllersProvider<T> _controllersProvider;
        private readonly IGameCommunicator _gameCommunicator;
        private readonly string _mainControllerName;

        public ControllerMiddleware(IControllersProvider<T> controllersProvider, IGameCommunicator gameCommunicator)
        {
            _controllersProvider = controllersProvider;
            _gameCommunicator = gameCommunicator;
            _mainControllerName = controllersProvider.MainControllerName;
        }

        public async Task Pipe(IncomingMessage message, PlayerInfo info, T data, Func<Task> next)
        {
            data.ControllersStack ??= new List<string>(new[] {_mainControllerName});
            
            string realControllerName = data.ControllersStack?.LastOrDefault() ?? _mainControllerName;

            ControllerBase<T> controller = _controllersProvider.ResolveControllerByName(realControllerName);
            IControllerAnswer answer = await controller.ProcessMessage(message, info, data);

            await ProcessAnswer(answer, data, info);
        }

        // ReSharper disable once UnusedParameter.Local
        private async Task ProcessAnswer(IControllerAnswer answer, T data, PlayerInfo info)
        {
            await _gameCommunicator.Answer(new GameAnswer
            {
                PlayerId = info.Key,
                Suggestions = answer.Suggestions,
                Text = answer.Text
            });
        }
    }
}