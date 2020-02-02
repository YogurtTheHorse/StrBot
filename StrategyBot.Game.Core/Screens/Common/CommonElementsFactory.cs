using System;
using System.Threading.Tasks;
using StrategyBot.Game.Logic.Communications;
using StrategyBot.Game.Logic.Localizations;
using StrategyBot.Game.Logic.Screens.Common.Elements;
using Action = StrategyBot.Game.Logic.Screens.Common.Elements.Action;

namespace StrategyBot.Game.Logic.Screens.Common
{
    public class CommonElementsFactory : ICommonElementsFactory
    {
        private readonly Lazy<ILocalizer> _localizer;
        private readonly Lazy<IGameCommunicator> _gameCommunicator;

        public CommonElementsFactory(
            Lazy<ILocalizer> localizer,
            Lazy<IGameCommunicator> gameCommunicator
        )
        {
            _localizer = localizer;
            _gameCommunicator = gameCommunicator;
        }

        public Greetings Greet(LocalizationDescription description) => 
            new Greetings(description, _localizer.Value, _gameCommunicator.Value);

        public Action Action(LocalizationDescription description, Func<Task<bool>> callback) => 
            new Action(description, callback, _localizer.Value);

        public NoAction NoAction(LocalizationDescription description) =>  
            new NoAction(description, _localizer.Value, _gameCommunicator.Value);
    }
}