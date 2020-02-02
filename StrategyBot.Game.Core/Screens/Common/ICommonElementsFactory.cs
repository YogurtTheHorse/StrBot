using System;
using System.Threading.Tasks;
using StrategyBot.Game.Logic.Localizations;
using StrategyBot.Game.Logic.Screens.Common.Elements;
using Action = StrategyBot.Game.Logic.Screens.Common.Elements.Action;

namespace StrategyBot.Game.Logic.Screens.Common
{
    public interface ICommonElementsFactory
    {
        Greetings Greet(LocalizationDescription description);

        Action Action(LocalizationDescription description, Func<Task<bool>> callback);

        NoAction NoAction(LocalizationDescription description);
    }
}