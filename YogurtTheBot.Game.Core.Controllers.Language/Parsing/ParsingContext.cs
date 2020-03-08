using YogurtTheBot.Game.Core.Controllers.Language.Expressions;

namespace YogurtTheBot.Game.Core.Controllers.Language.Parsing
{
    public class ParsingContext
    {
        public string Text { get; }

        public int Position { get; }

        public ParsingContext(string text, int position = 0)
        {
            Text = text;
            Position = position;
        }

        public ParsingContext Change(int position) =>
            new ParsingContext(Text, position);

        public bool ContinuesWith(string value) => Text.Substring(Position).StartsWith(value);

        public ParsingContext AppendTerminal(Terminal terminal) => 
            Change(Position + terminal.Value.Length);
    }
}