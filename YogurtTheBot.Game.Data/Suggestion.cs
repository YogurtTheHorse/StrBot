namespace YogurtTheBot.Game.Data
{
    public class Suggestion
    {
        public string Text { get; set; }

        public static implicit operator Suggestion(string s) => new Suggestion{ Text = s };
    }
}