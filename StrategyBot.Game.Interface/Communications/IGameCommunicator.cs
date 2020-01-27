using System.Threading.Tasks;

namespace StrategyBot.Game.Interface
{
    public interface IGameCommunicator
    {
        Task Answer(GameAnswer message, GameMessageType messageType = GameMessageType.RegularAnswer);
    }
}