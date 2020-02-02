using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyBot.Game.Logic.Screens;
using StrategyBot.Game.Logic.Screens.Common;

namespace StrategyBot.Game.Screens
{
    [MainScreen]
    public class MainMenuScreen : CommonScreen
    {
        public MainMenuScreen(ICommonElementsFactory commonElementsFactory) : base(commonElementsFactory)
        {
        }

        protected override IEnumerable<CommonScreenElement> ScreenElements => new CommonScreenElement[]
        {
            CommonElementsFactory.Action("screens.main_menu.attack", () => Task.FromResult(false)),
            CommonElementsFactory.NoAction("screens.main_menu.open_phrase"),
        };
    }
}