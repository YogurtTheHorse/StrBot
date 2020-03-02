namespace YogurtTheBot.Game.Core.Localizations
{
    public interface ILocalizer
    {
        Localization GetString(string key, string locale);
    }
}