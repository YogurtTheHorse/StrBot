using System;

namespace StrategyBot.Game.Core.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
        public string ControllerName { get; }
        public bool IsMainController { get; }

        public ControllerAttribute(string controllerName = null, bool isMainController = false)
        {
            ControllerName = controllerName;
            IsMainController = isMainController;
        }
    }
}