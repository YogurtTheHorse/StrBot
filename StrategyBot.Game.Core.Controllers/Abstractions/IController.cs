using System.Reflection;
using StrategyBot.Game.Core.Controllers.Handlers;

namespace StrategyBot.Game.Core.Controllers.Abstractions
{
    // TODO: Remove IController and use reflection.
    public interface IController
    {
         (MethodInfo methodInfo, ActionAttribute actionAttribute)[] ActionsInfos { get; }

         (MethodInfo methodInfo, GreetingsAttribute greetingsAttribute)? GreetingInfo { get; }

         (MethodInfo methodInfo, DefaultAttribute defaultAttribute)? DefaultInfo { get; }
    }
}