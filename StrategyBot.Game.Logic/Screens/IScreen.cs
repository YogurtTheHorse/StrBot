using System.Threading.Tasks;
using StrategyBot.Game.Logic.Entities;

namespace StrategyBot.Game.Logic.Screens
{
    public interface IScreen
    {
        Task ProcessMessage(IncomingMessage message);
    }
}