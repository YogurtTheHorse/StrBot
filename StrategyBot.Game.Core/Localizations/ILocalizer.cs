namespace StrategyBot.Game.Logic.Localizations
{
    public interface ILocalizer
    {
        Localization GetString(string key, string locale);
    }
}