using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyBot.Game.Logic.Localizations;
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
            CommonElementsFactory.Action(
                LocalizationPath.Root.Screens.MainMenu.OpenPhrase.Attack() as LocalizationDescription,
                () => Task.FromResult(false)
            ),
            CommonElementsFactory.NoAction(
                LocalizationPath.Root.Screens.MainMenu.OpenPhrase()
            ),
        };
    }
}