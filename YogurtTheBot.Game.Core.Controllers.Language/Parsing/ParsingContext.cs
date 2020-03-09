using System;
using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Language.Parsing
{
    public class ParsingContext
    {
        public string Text { get; }

        public int Position { get; }

        public ILocalizer Localizer { get; set; }

        public string Locale { get; set; }

        public ParsingContext(string text, int position = 0)
        {
            Text = text;
            Position = position;
        }

        public ParsingContext Change(int position) =>
            new ParsingContext(Text, position)
            {
                Localizer = Localizer, Locale = Locale
            };

        public bool ContinuesWith(string value) => Text
            .Substring(Position)
            .StartsWith(value, StringComparison.OrdinalIgnoreCase);

        public ParsingContext AppendTerminal(Terminal terminal) =>
            Change(Position + terminal.Value.Length);
    }
}