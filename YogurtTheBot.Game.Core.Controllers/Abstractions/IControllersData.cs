using System.Collections.Generic;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public interface IControllersData
    {
        Stack<string> ControllersStack { get; }
    }
}