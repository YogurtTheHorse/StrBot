using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using YogurtTheBot.Game.Core.Controllers.Abstractions;

namespace YogurtTheBot.Game.Core.Controllers.Autofac
{
    public static class AutofacExtensions
    {
        public static void RegisterControllers<T>(this ContainerBuilder containerBuilder, Assembly controllersAssembly)
            where T : IControllersData
        {
            IEnumerable<Type> types = Controllers.GetControllersTypesInAssembly(controllersAssembly);

            foreach (Type controllerType in types)
            {
                if (!typeof(Controller<T>).IsAssignableFrom(controllerType))
                {
                    throw new InvalidOperationException($"{controllerType.Name} should implement IController<T>.");
                }

                // ReSharper disable once SuggestVarOrType_Elsewhere
                var registration =
                    containerBuilder
                        .RegisterType(controllerType)
                        .Named<Controller<T>>(Controllers.GetName(controllerType));

                if (Controllers.IsMain(controllerType))
                {
                    registration.Named<Controller<T>>(AutofacControllersProvider<T>.MainControllerAutofacName);
                }
            }

            containerBuilder
                .RegisterType<AutofacControllersProvider<T>>()
                .As<IControllersProvider<T>>();
        }
    }
}