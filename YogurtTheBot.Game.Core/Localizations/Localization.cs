using System;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using YogurtTheBot.Game.Core.Communications;

namespace YogurtTheBot.Game.Core.Localizations
{
    public class Localization
    {
        private readonly Random _random;

        public Localization(string[] formats, Random random = null)
        {
            _random = random;
            Values = formats;
        }

        public Localization Format(params object[] args) =>
            args.Length > 0
                ? new Localization(Values.Select(f => string.Format(f, args)).ToArray(), _random)
                : this;

        public string Value => Values[_random.Next(0, Values.Length - 1)];

        public string[] Values { get; }

        public bool MatchesMessage(string message, params object[] args)
        {
            string preparedMessage = PrepareForCompartment(message);
            
            return Format(args)
                .Values
                .Any(f => PrepareForCompartment(f).Equals(preparedMessage, StringComparison.OrdinalIgnoreCase));
        }

        private string PrepareForCompartment(string s) =>
            string.Join("", s
                .ToArray()
                .Where(c => !char.IsPunctuation(c))
            ).Trim();
    }
}