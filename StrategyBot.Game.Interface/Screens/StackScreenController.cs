using System;
using System.Collections.Generic;
using System.Linq;
using StrategyBot.Game.Interface.Models;

namespace StrategyBot.Game.Interface.Screens
{
    public class StackScreenController : IScreenController
    {
        private readonly Dictionary<string, IScreen> _screens;
        private readonly string _mainScreenName;

        public StackScreenController(IEnumerable<IScreen> screens)
        {
            _screens = screens.ToDictionary(s => s.GetType().Name);

            foreach ((string name, IScreen screen) in _screens)
            {
                if (!Attribute.IsDefined(screen.GetType(), typeof(MainScreenAttribute))) continue;

                _mainScreenName = name;
                break;
            }

            if (_mainScreenName is null)
            {
                throw new InvalidOperationException("No main screen was defined");
            }
        }

        public IScreen GetCurrentPlayerScreen(PlayerState playerState)
        {
            if (playerState.ScreensStack.TryPeek(out string currentScreenName))
            {
                if (!_screens.TryGetValue(currentScreenName, out IScreen currentScreen))
                    throw new InvalidOperationException(
                        $"Screen with name {currentScreenName} wasn't found. " +
                        $"Check {playerState.Key} player data"
                    );

                return currentScreen;
            }

            playerState.ScreensStack.Push(_mainScreenName);
            return _screens[_mainScreenName];
        }

        public void OpenScreen<T>(PlayerState playerState, bool safeOpen) where T : IScreen
        {
            if (safeOpen) // check screen existence
            {
                bool screenExists = _screens.ContainsKey(typeof(T).Name);

                if (!screenExists)
                {
                    throw new InvalidOperationException(
                        $"Screen with name {typeof(T).Name} wasn't found. Neither check screen registration " +
                        "or use unsafe open (what's not recommended obviously)."
                    );
                }
            }

            playerState.ScreensStack.Push(typeof(T).Name);
        }

        public void Back(PlayerState playerState)
        {
            if (!playerState.ScreensStack.TryPop(out string _))
            {
                playerState.ScreensStack.Push(_mainScreenName);
            }
        }
    }
}