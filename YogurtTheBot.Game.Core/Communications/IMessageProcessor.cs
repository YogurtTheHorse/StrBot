using System.Threading.Tasks;

namespace YogurtTheBot.Game.Core.Communications
{
    public interface IMessageProcessor<T>
    {
        Task ProcessMessage(IncomingMessage message, PlayerInfo info, T data);
    }
}