using System.Threading.Tasks;
using StrategyBot.Game.Logic.Communications;

namespace StrategyBot.Game.Logic.Screens
{
    public interface IScreen
    {
        Task ProcessMessage(IncomingMessage message, PlayerState state, PlayerData data);

        Task OnOpen(PlayerState state, PlayerData data);
    }
}