using System.Reflection;
using StrategyBot.Game.Core.Models;

namespace StrategyBot.Game.Core.Screens
{
    public interface IScreenController
    {
        IScreen GetCurrentPlayerScreen(PlayerState playerState);

        void OpenScreen<T>(PlayerState playerState, bool safeOpen = true) where T : IScreen;

        void ReplaceScreen<T>(PlayerState playerState) where T : IScreen
        {
            Back(playerState);
            OpenScreen<T>(playerState);
        }

        void Back(PlayerState playerState);
    }
}