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

        public Greetings Greet(string greetKey) => new Greetings(greetKey, _localizer.Value, _gameCommunicator.Value);

        public Action Action(string key, Func<Task<bool>> callback) => new Action(key, callback, _localizer.Value);

        public NoAction NoAction(string key) =>  new NoAction(key, _localizer.Value, _gameCommunicator.Value);
    }
}