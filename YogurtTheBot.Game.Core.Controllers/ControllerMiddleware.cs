using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Communications.Pipeline;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Answers;
using YogurtTheBot.Game.Core.Controllers.Handlers;
using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers
{
    public class ControllerMiddleware<T> : IMiddleware<T> where T : IControllersData
    {
        private readonly IControllersProvider<T> _controllersProvider;
        private readonly ILocalizer _localizer;
        private readonly IGameCommunicator _gameCommunicator;
        private readonly string _mainControllerName;

        public ControllerMiddleware(IControllersProvider<T> controllersProvider, ILocalizer localizer, IGameCommunicator gameCommunicator)
        {
            _controllersProvider = controllersProvider;
            _localizer = localizer;
            _gameCommunicator = gameCommunicator;
            _mainControllerName = controllersProvider.MainControllerName;
        }

        public async Task Pipe(IncomingMessage message, PlayerInfo info, T data, Func<Task> next)
        {
            string realControllerName = data.ControllersStack.TryPeek(out string controllerName)
                ? controllerName
                : _mainControllerName;

            Controller<T> controller = _controllersProvider.ResolveControllerByName(realControllerName);
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