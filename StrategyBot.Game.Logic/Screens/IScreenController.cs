using System.Reflection;
using StrategyBot.Game.Logic.Models;

namespace StrategyBot.Game.Logic.Screens
{
    public interface IScreenController
    {
        IScreen GetCurrentPlayerScreen(PlayerData playerData);

        void OpenScreen<T>(PlayerData playerData, bool safeOpen = true) where T : IScreen;

        void ReplaceScreen<T>(PlayerData playerData) where T : IScreen
        {
            Back(playerData);
            OpenScreen<T>(playerData);
        }

        void Back(PlayerData playerData);
    }
}