using System.Collections.Generic;

namespace StrategyBot.Game.Core.Controllers
{
    public interface IControllersData
    {
        Stack<string> ControllersStack { get; }
    }
}