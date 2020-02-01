using System.Threading.Tasks;

namespace StrategyBot.Game.Core
{
    public interface IGameCommunicator
    {
        Task Answer(GameAnswer message, GameMessageType messageType = GameMessageType.RegularAnswer);
    }
}