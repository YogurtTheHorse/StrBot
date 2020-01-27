namespace StrategyBot.Game.Logic
{
    public interface ILocalizer
    {
        string GetString(string key, string locale, params object[] args);
    }
}