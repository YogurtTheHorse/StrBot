using System.Collections.Generic;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public interface IControllersData
    {
        List<string> ControllersStack { get; set; }
    }
}