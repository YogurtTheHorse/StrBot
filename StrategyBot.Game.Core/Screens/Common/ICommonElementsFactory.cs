using System;
using System.Threading.Tasks;
using StrategyBot.Game.Logic.Screens.Common.Elements;
using Action = StrategyBot.Game.Logic.Screens.Common.Elements.Action;

namespace StrategyBot.Game.Logic.Screens.Common
{
    public interface ICommonElementsFactory
    {
        Greetings Greet(string greetKey);

        Action Action(string key, Func<Task<bool>> callback);

        NoAction NoAction(string key);
    }
}