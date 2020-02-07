using System.Threading.Tasks;

namespace StrategyBot.Game.Core.Communications
{
    public interface IMessageProcessor
    {
        Task ProcessMessage(IncomingMessage message, PlayerState state, PlayerData data);
    }
}