using System.Reflection;
using StrategyBot.Game.Interface.Models;

namespace StrategyBot.Game.Interface.Screens
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