using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrategyBot.Game.Data;
using StrategyBot.Game.Logic.Communications;
using StrategyBot.Game.Logic.Localizations;

namespace StrategyBot.Game.Logic.Screens.Common
{
    public abstract class CommonScreen : IScreen
    {
        protected readonly ICommonElementsFactory CommonElementsFactory;

        protected abstract IEnumerable<CommonScreenElement> ScreenElements { get; }

        protected CommonScreen(ICommonElementsFactory commonElementsFactory)
        {
            CommonElementsFactory = commonElementsFactory;
        }
        
        public async Task ProcessMessage(IncomingMessage message, PlayerState state, PlayerData data)
        {
            foreach (CommonScreenElement element in ScreenElements)
            {
                if (await element.ProcessMessage(message, state, data, this))
                {
                    return;
                }
            }
        }

        public async Task OnOpen(PlayerState state, PlayerData data)
        {
            foreach (CommonScreenElement element in ScreenElements)
            {
                if (await element.OnOpen(state, data, this))
                {
                    return;
                }
            }
        }

        public Suggestion[] GetSuggestions(PlayerState state, PlayerData data) =>
            ScreenElements
                .SelectMany(e => e.GetSuggestions(state, data))
                .ToArray();
    }
}