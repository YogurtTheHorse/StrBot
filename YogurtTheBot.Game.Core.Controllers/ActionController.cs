using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Answers;
using YogurtTheBot.Game.Core.Controllers.Handlers;
using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers
{
    public abstract class ActionController<T> : ControllerBase<T> where T : IControllersData
    {
        protected readonly ActionHandler<T>[] ActionHandlers;

        protected ActionController(IControllersProvider<T> controllersProvider, ILocalizer localizer) 
            : base(controllersProvider, localizer)
        {
            ActionHandlers = (
                    from methodInfo in GetType().GetMethods()
                    let attribute = Attribute.GetCustomAttribute(methodInfo, typeof(ActionAttribute)) as ActionAttribute
                    where !(attribute is null)
                    select new ActionHandler<T>(attribute.LocalizationPath, localizer, methodInfo)
                )
                .ToArray();
        }

        public override async Task<IControllerAnswer> ProcessMessage(IncomingMessage message, PlayerInfo info, T data)
        {
            IMessageHandler<T>[] handlers = GetHandlers()
                .OrderByDescending(h => h.Priority)
                .ToArray();

            foreach (IMessageHandler<T> handler in handlers)
            {
                IControllerAnswer? answer = await handler.Handle(this, message, info, data);

                if (answer != null) return answer;
            }

            return DefaultHandler(message, info, data);
        }

        protected virtual IControllerAnswer DefaultHandler(IncomingMessage message, PlayerInfo info, T data)
        {
            throw new NotImplementedException();
        }


        protected virtual IEnumerable<IMessageHandler<T>> GetHandlers() => ActionHandlers;

        protected override IControllerSuggestion[] GetSuggestions()
        {
            return ActionHandlers
                .Select(h => new LocalizedSuggestion(h.LocalizationPath.Path))
                .Cast<IControllerSuggestion>()
                .ToArray();
        }

        protected virtual IControllerAnswer Answer(string text) =>
            new ControllerAnswer
            {
                Suggestions = GetSuggestions(),
                Text = text
            };
    }
}