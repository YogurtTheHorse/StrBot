using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using StrategyBot.Game.Core.Controllers;
using StrategyBot.Game.Core.Controllers.Abstractions;
using StrategyBot.Game.Logic.Communications.Pipeline;

namespace StrategyBot.Game.Core.Controllers.Autofac
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
                .RegisterType<ControllerMiddleware>()
                .As<IMiddleware>();

            containerBuilder
                .RegisterType<AutofacControllersProvider>()
                .As<IControllersProvider>();
        }
    }
}