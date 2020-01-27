using System.Threading.Tasks;
using StrategyBot.Game.Interface.Entities;
using StrategyBot.Game.Interface.Models;

namespace StrategyBot.Game.Interface.Screens
{
    public interface IScreen
    {
        Task ProcessMessage(IncomingMessage message, PlayerState playerState);
    }
}