using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyBot.Game.Logic.Screens
{
    public class StackScreenController : IScreenController
    {
        private readonly IScreen[] _screens;
        private readonly string _mainScreenName;

        public StackScreenController(IEnumerable<IScreen> screens)
        {
            _screens = screens.ToArray();

            foreach (IScreen screen in _screens)
            {
                if (!Attribute.IsDefined(screen.GetType(), typeof(MainScreenAttribute))) continue;
                
                _mainScreenName = screen.GetType().Name;
                break;
            }

            if (_mainScreenName is null)
            {
                throw new InvalidOperationException("No main screen was defined");
            }
        }
    }
}