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
        protected IMessageHandler<T>[] ActionHandlers;

        protected Controller(ILocalizer localizer)
        {
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

            return await DefaultHandler(message, info, data);
        }

        protected virtual Task<IControllerAnswer> DefaultHandler(IncomingMessage message, PlayerInfo info, T data)
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
    }
}