using System.Threading.Tasks;

namespace StrategyBot.Game.Logic.Communications
{
    public interface IMessageProcessor
    {
        Task ProcessMessage(IncomingMessage message, PlayerState state, PlayerData data);
    }
}