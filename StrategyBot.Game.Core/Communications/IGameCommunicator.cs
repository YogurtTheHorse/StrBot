using System.Threading.Tasks;
using StrategyBot.Game.Data;

namespace StrategyBot.Game.Core.Communications
{
    public interface IGameCommunicator
    {
        Task Answer(GameAnswer message, GameMessageType messageType = GameMessageType.RegularAnswer);
    }
}