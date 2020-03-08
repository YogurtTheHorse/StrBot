using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Answers;

namespace YogurtTheBot.Game.Core.Controllers.Handlers
{
    public abstract class BaseMessageHandler<T> : IMessageHandler<T> where T : IControllersData
    {
        private readonly MethodInfo _methodInfo;

        protected BaseMessageHandler(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }

        public abstract bool CanHandle(IncomingMessage message, PlayerInfo playerInfo);

        public async Task<IControllerAnswer> Handle(
            Controller<T> controller,
            IncomingMessage message,
            PlayerInfo info,
            T data)
        {
            object[] parameters = BuildParameters(message, info, data, controller);

            if (typeof(Task<IControllerAnswer>).IsAssignableFrom(_methodInfo.ReturnType))
            {
                var task = (Task<IControllerAnswer>) _methodInfo.Invoke(controller, parameters);
                await task;
            }
            else if (typeof(IControllerAnswer).IsAssignableFrom(_methodInfo.ReturnType))
            {
                return (IControllerAnswer) _methodInfo.Invoke(controller, parameters);
            }

            throw new InvalidOperationException("TODO");
        }

        public virtual int Priority => 0;

        private object[] BuildParameters(params object[] availableParameters)
        {
            IEnumerable<Type> parametersTypes = _methodInfo.GetParameters().Select(p => p.ParameterType);

            return (
                from parameterType in parametersTypes
                from availableParameter in availableParameters
                where parameterType.IsInstanceOfType(availableParameter)
                select availableParameter
            ).ToArray();
        }
    }
}