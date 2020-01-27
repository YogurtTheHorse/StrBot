using System.Threading.Tasks;

namespace StrategyBot.Game.Logic
{
    public interface IGameCommunicator
    {
        Task Answer(GameAnswer message, GameMessageType messageType = GameMessageType.RegularAnswer);
    }
}