using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Controllers.Answers;
using YogurtTheBot.Game.Core.Controllers.Handlers;
using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public abstract class Controller<T> where T : IControllersData
    {
        protected readonly IControllersProvider<T> ControllersProvider;
        protected readonly IMessageHandler<T>[] ActionHandlers;

        protected Controller(IControllersProvider<T> controllersProvider, ILocalizer localizer)
        {
            ControllersProvider = controllersProvider;
            
            ActionHandlers = (
                    from methodInfo in GetType().GetMethods()
                    let attribute = Attribute.GetCustomAttribute(methodInfo, typeof(ActionAttribute)) as ActionAttribute
                    where !(attribute is null)
                    select new ActionHandler<T>(attribute.LocalizationPath, localizer, methodInfo)
                )
                .Cast<IMessageHandler<T>>()
                .ToArray();
        }

        public async Task<IControllerAnswer> ProcessMessage(IncomingMessage message, PlayerInfo info, T data)
        {
            foreach (IMessageHandler<T> handler in GetHandlers())
            {
                if (handler.CanHandle(message, info))
                {
                    return await handler.Handle(this, message, info, data);
                }
            }

            return DefaultHandler(message, info, data);
        }

        protected virtual IControllerAnswer DefaultHandler(IncomingMessage message, PlayerInfo info, T data)
        {
            throw new NotImplementedException();
        }

        protected virtual IControllerAnswer OnOpen(PlayerInfo info, T data)
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerable<IMessageHandler<T>> GetHandlers() => ActionHandlers;

        protected virtual IControllerAnswer Answer(string text) =>
            new ControllerAnswer
            {
                Suggestions = GetSuggestions(),
                Text = text
            };

        protected virtual Suggestion[] GetSuggestions()
        {
            return Array.Empty<Suggestion>();
        }

        protected IControllerAnswer Open(string controllerName, PlayerInfo info, T data)
        {
            Controller<T> controller = ControllersProvider.ResolveControllerByName(controllerName);
            data.ControllersStack.Append(controllerName);

            return controller.OnOpen(info, data);
        }

        protected IControllerAnswer Back(PlayerInfo info, T data)
        {
            data.ControllersStack.Pop();
            
            string currentControllerName = data.ControllersStack.Peek();
            Controller<T> controller = ControllersProvider.ResolveControllerByName(currentControllerName);

            return controller.OnOpen(info, data);
        }
    }
}