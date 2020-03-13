using System;
using System.Linq;
using System.Threading.Tasks;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Controllers.Answers;
using YogurtTheBot.Game.Core.Localizations;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public abstract class ControllerBase<T> where T : IControllersData
    {
        protected IControllersProvider<T> ControllersProvider { get; }
        protected ILocalizer Localizer { get; }

        protected ControllerBase(IControllersProvider<T> controllersProvider, ILocalizer localizer)
        {
            ControllersProvider = controllersProvider;
            Localizer = localizer;
        }

        public abstract Task<IControllerAnswer> ProcessMessage(IncomingMessage message, PlayerInfo info, T data);
        
        protected virtual Suggestion[] GetSuggestions()
        {
            return Array.Empty<Suggestion>();
        }

        protected virtual IControllerAnswer OnOpen(PlayerInfo info, T data)
        {
            throw new NotImplementedException();
        }

        protected IControllerAnswer Open(string controllerName, PlayerInfo info, T data)
        {
            ControllerBase<T> controller = ControllersProvider.ResolveControllerByName(controllerName);
            data.ControllersStack.Add(controllerName);

            return controller.OnOpen(info, data);
        }

        protected IControllerAnswer Back(PlayerInfo info, T data)
        {
            if (data.ControllersStack.Count > 0)
            {
                data.ControllersStack.RemoveAt(data.ControllersStack.Count - 1);
            }

            string currentControllerName =
                data.ControllersStack.LastOrDefault() ?? ControllersProvider.MainControllerName;
            ControllerBase<T> controller = ControllersProvider.ResolveControllerByName(currentControllerName);

            return controller.OnOpen(info, data);
        }
    }
}