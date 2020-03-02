namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public interface IControllersProvider
    {
        IController ResolveControllerByName(string s);

        string MainControllerName { get; }
    }
}