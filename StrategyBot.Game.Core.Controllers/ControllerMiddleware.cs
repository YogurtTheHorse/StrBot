using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using StrategyBot.Game.Core.Controllers.Abstractions;
using StrategyBot.Game.Core.Controllers.Answers;
using StrategyBot.Game.Core.Controllers.Handlers;
using StrategyBot.Game.Data;
using StrategyBot.Game.Logic;
using StrategyBot.Game.Logic.Communications;
using StrategyBot.Game.Logic.Communications.Pipeline;
using StrategyBot.Game.Logic.Localizations;

namespace StrategyBot.Game.Core.Controllers
{
    public class ControllerMiddleware : IMiddleware
    {
        private readonly IControllersProvider _controllersProvider;
        private readonly ILocalizer _localizer;
        private readonly IGameCommunicator _gameCommunicator;
        private readonly string _mainControllerName;

        public ControllerMiddleware(IControllersProvider controllersProvider, ILocalizer localizer, IGameCommunicator gameCommunicator)
        {
            _controllersProvider = controllersProvider;
            _localizer = localizer;
            _gameCommunicator = gameCommunicator;
            _mainControllerName = controllersProvider.MainControllerName;
        }

        public async Task Pipe(IncomingMessage message, PlayerState state, PlayerData data, Func<Task> next)
        {
            string realControllerName = state.ControllersStack.TryPeek(out string controllerName)
                ? controllerName
                : _mainControllerName;

            IController controller = _controllersProvider.ResolveControllerByName(realControllerName);
            IControllerAnswer answer = null;

            foreach ((MethodInfo methodInfo, ActionAttribute actionAttribute) in controller.ActionsInfos)
            {
                Localization actionString = _localizer.GetString(actionAttribute.LocalizationPath.Path, state.Locale);

                if (!actionString.MatchesMessage(message)) continue;

                IEnumerable<object> parameters = BuildParameters(methodInfo, message, state, data);

                answer = await CallHandler(controller, methodInfo, parameters.ToArray());
                break;
            }

            if (answer is null && controller.DefaultInfo != null)
            {
                IEnumerable<object> parameters = BuildParameters(
                    controller.DefaultInfo.Value.methodInfo,
                    message, state, data
                );

                answer = await CallHandler(controller, controller.DefaultInfo.Value.methodInfo, parameters.ToArray());
            }

            await ProcessAnswer(answer, data, state);
        }

        private async Task ProcessAnswer(IControllerAnswer answer, PlayerData data, PlayerState state)
        {
            await _gameCommunicator.Answer(new GameAnswer
            {
                PlayerId = data.Key,
                Suggestions = answer.Suggestions,
                Text = answer.Text
            });
        }

        private static IEnumerable<object> BuildParameters(MethodBase methodInfo, params object[] availableParameters)
        {
            IEnumerable<Type> parametersTypes = methodInfo.GetParameters().Select(p => p.ParameterType);

            return
                from parameterType in parametersTypes
                from availableParameter in availableParameters
                where parameterType.IsInstanceOfType(availableParameter)
                select availableParameter;
        }

        private static async Task<IControllerAnswer> CallHandler(
            IController controller,
            MethodInfo methodInfo,
            object[] parameters
        )
        {
            if (typeof(Task<IControllerAnswer>).IsAssignableFrom(methodInfo.ReturnType))
            {
                var task = (Task<IControllerAnswer>) methodInfo.Invoke(controller, parameters);
                await task;
            }
            else if (typeof(IControllerAnswer).IsAssignableFrom(methodInfo.ReturnType))
            {
                return (IControllerAnswer) methodInfo.Invoke(controller, parameters);
            }

            throw new InvalidOperationException("TODO");
        }
    }
}