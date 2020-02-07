using System;

namespace StrategyBot.Game.Core.Controllers.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GreetingsAttribute : Attribute, IHandlerAttribute
    {
        
    }
}