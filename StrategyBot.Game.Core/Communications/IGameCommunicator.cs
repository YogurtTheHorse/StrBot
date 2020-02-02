using System.Threading.Tasks;
using StrategyBot.Game.Data;

namespace StrategyBot.Game.Logic.Communications
{
    public interface IGameCommunicator
    {
        Task Answer(GameAnswer message, GameMessageType messageType = GameMessageType.RegularAnswer);
    }
}