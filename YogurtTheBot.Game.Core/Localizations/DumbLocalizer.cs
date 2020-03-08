using System.Collections.Generic;

namespace YogurtTheBot.Game.Core.Localizations
{
    public class DumbLocalizer : ILocalizer
    {
        private readonly Dictionary<string, string[]> _mappings;

        public DumbLocalizer(Dictionary<string, string[]> mappings)
        {
            _mappings = mappings;
        }

        public Localization GetString(string key, string locale) =>
            _mappings.TryGetValue(key, out string[] values)
                ? new Localization(values)
                : new Localization(new[] {key});
    }
}