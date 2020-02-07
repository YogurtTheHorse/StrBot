using System;
using System.Linq;
using StrategyBot.Game.Core.Communications;

namespace StrategyBot.Game.Core.Localizations
{
    public class Localization
    {
        private readonly Random _random;
        private readonly string[] _formats;

        public Localization(Random random, string[] formats)
        {
            _random = random;
            _formats = formats;
        }

        public Localization Format(params object[] args) =>
            args.Length > 0
                ? new Localization(
                    _random,
                    _formats.Select(f => string.Format(f, args)).ToArray()
                )
                : this;

        public string Value => _formats[_random.Next(0, _formats.Length - 1)];

        public bool MatchesMessage(IncomingMessage message, params object[] args)
        {
            return Format(args)
                ._formats
                .Any(f => f.Equals(message.Text));
        }
    }
}