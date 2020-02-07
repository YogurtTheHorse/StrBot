namespace StrategyBot.Game.Logic.Localizations
{
    public class LocalizationPath
    {
        public string Path { get; }

        protected LocalizationPath(string path)
        {
            Path = path;
        }

        public static implicit operator LocalizationPath(string path) => new LocalizationPath(path);
    }
}