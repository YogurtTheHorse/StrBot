namespace StrategyBot.Game.Data
{
    public class Suggestion
    {
        private readonly string _suggestion;

        public Suggestion(string suggestion)
        {
            _suggestion = suggestion;
        }

        public static implicit operator Suggestion(string s) => new Suggestion(s);
    }
}