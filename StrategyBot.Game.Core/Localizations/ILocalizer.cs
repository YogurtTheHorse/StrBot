namespace StrategyBot.Game.Core.Localizations
{
    public interface ILocalizer
    {
        Localization GetString(string key, string locale);
    }
}