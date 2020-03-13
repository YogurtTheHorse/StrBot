using Autofac;
using YogurtTheBot.Game.Core.Controllers.Abstractions;

namespace YogurtTheBot.Game.Core.Controllers.Autofac
{
    public class AutofacControllersProvider<T> : IControllersProvider<T> where T : IControllersData 
    {
        internal const string MainControllerAutofacName = "MAIN_CONTROLLER_NAME";
        private readonly ILifetimeScope _scope;

        public AutofacControllersProvider(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public ControllerBase<T> ResolveControllerByName(string s) => _scope.ResolveNamed<ControllerBase<T>>(s);

        public string MainControllerName => MainControllerAutofacName;
    }
}