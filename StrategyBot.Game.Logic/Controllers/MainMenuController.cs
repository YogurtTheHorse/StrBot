using StrategyBot.Game.Core.Controllers;
using StrategyBot.Game.Core.Controllers.Abstractions;
using StrategyBot.Game.Core.Controllers.Answers;
using StrategyBot.Game.Core.Controllers.Handlers;
using StrategyBot.Game.Logic;

namespace StrategyBot.Game.Controllers
{
    [Controller(isMainController: true)]
    public class MainMenuController : ControllerBase
    {
        [Action("screens.main_menu.attack")]
        public IControllerAnswer Test()
        {
            return Answer("test action");
        }

        [Default]
        public IControllerAnswer Default(PlayerState player)
        {
            return Answer("hi, " + player.SocialId);
        }
    }
}