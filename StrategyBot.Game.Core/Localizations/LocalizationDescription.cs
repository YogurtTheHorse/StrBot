using System;

namespace StrategyBot.Game.Core.Localizations
{
    public class LocalizationDescription
    {
        public string Key { get; }

        public Func<PlayerState, PlayerData, object[]> ArgsFactory { get; }

        public LocalizationDescription(string key)
            : this(key, (ps, pd) => Array.Empty<object>())
        {
        }

        public LocalizationDescription(string key, Func<PlayerState, PlayerData, object[]> argsFactory)
        {
            Key = key;
            ArgsFactory = argsFactory;
        }
    }
}