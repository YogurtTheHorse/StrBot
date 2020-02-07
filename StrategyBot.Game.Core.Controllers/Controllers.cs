using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StrategyBot.Game.Core.Controllers
{
    public static class Controllers
    {
        public static string GetName(Type controllerType)
        {
            var controllerAttribute = (ControllerAttribute) Attribute.GetCustomAttribute(
                controllerType,
                typeof(ControllerAttribute)
            );

            return controllerAttribute.ControllerName ?? controllerType.Name;
        }

        public static IEnumerable<Type> GetControllersTypesInAssembly(Assembly controllersAssembly)
        {
            return controllersAssembly
                .GetTypes()
                .Where(t => Attribute.IsDefined(t, typeof(ControllerAttribute)));
        }

        public static bool IsMain(Type controllerType)
        {
            var controllerAttribute = (ControllerAttribute) Attribute.GetCustomAttribute(
                controllerType,
                typeof(ControllerAttribute)
            );

            return controllerAttribute.IsMainController;
        }
    }
}