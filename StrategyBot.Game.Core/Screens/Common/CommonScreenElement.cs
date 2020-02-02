using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StrategyBot.Game.Data;
using StrategyBot.Game.Logic.Communications;

namespace StrategyBot.Game.Logic.Screens.Common
{
    public class CommonScreenElement
    {
        public virtual Task<bool> OnOpen(PlayerState state, PlayerData data, CommonScreen screen) =>
            Task.FromResult(false);

        public virtual Task<bool> ProcessMessage(
            IncomingMessage message,
            PlayerState state,
            PlayerData data,
            CommonScreen screen
        ) => Task.FromResult(false);

        public virtual IEnumerable<Suggestion> GetSuggestions(PlayerState state, PlayerData data) =>
            Array.Empty<Suggestion>();
    }
}