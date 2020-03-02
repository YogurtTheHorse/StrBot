namespace YogurtTheBot.Game.Data
{
    public class Suggestion
    {
        public string Suggestion1 { get; }

        public Suggestion(string suggestion)
        {
            Suggestion1 = suggestion;
        }

        public static implicit operator Suggestion(string s) => new Suggestion(s);
    }
}