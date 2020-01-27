using System.Threading.Tasks;
using StrategyBot.Game.Logic.Entities;
using StrategyBot.Game.Logic.Models;

namespace StrategyBot.Game.Logic.Screens
{
    public interface IScreen
    {
        Task ProcessMessage(IncomingMessage message, PlayerData playerData);
    }
}