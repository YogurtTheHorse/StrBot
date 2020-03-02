using System.Threading.Tasks;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Communications
{
    public interface IGameCommunicator
    {
        Task Answer(GameAnswer message, GameMessageType messageType = GameMessageType.RegularAnswer);
    }
}