using System;
using System.Threading.Tasks;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Answers;

namespace YogurtTheBot.Game.Core.Controllers.Handlers
{
    public interface IMessageHandler<T> where T : IControllersData
    {
        Task<IControllerAnswer?> Handle(
            Controller<T> controller,
            IncomingMessage message,
            PlayerInfo info,
            T data
        );

        int Priority { get; }
    }
}