using System.Threading.Tasks;
using StrategyBot.Game.Core.Entities;
using StrategyBot.Game.Core.Models;

namespace StrategyBot.Game.Core.Screens
{
    public interface IScreen
    {
        Task ProcessMessage(IncomingMessage message, PlayerState playerState, PlayerData playerData);

        Task OnOpen(PlayerState playerState, PlayerData playerData);
    }
}