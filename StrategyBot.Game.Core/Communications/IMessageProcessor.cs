using System.Threading.Tasks;

namespace StrategyBot.Game.Core.Communications
{
    public interface IMessageProcessor<T>
    {
        Task ProcessMessage(IncomingMessage message, PlayerInfo info, T data);
    }
}