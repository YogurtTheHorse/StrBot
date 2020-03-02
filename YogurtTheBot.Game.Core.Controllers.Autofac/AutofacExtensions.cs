using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using YogurtTheBot.Game.Core.Controllers.Abstractions;

namespace YogurtTheBot.Game.Core.Controllers.Autofac
{
    public static class AutofacExtensions
    {
        public static void RegisterControllers(this ContainerBuilder containerBuilder, Assembly controllersAssembly)
        {
            IEnumerable<Type> types = Controllers.GetControllersTypesInAssembly(controllersAssembly);

            foreach (Type controllerType in types)
            {
                if (!typeof(IController).IsAssignableFrom(controllerType))
                {
                    throw new InvalidOperationException($"{controllerType.Name} should implement IController.");
                }

                // ReSharper disable once SuggestVarOrType_Elsewhere
                var registration =
                    containerBuilder
                        .RegisterType(controllerType)
                        .Named<IController>(Controllers.GetName(controllerType));

                if (Controllers.IsMain(controllerType))
                {
                    registration.Named<IController>(AutofacControllersProvider.MainControllerAutofacName);
                }
            }

            containerBuilder
                .RegisterType<AutofacControllersProvider>()
                .As<IControllersProvider>();
        }
    }
}