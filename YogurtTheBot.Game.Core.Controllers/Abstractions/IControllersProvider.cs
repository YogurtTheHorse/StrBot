namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public interface IControllersProvider<T> where T : IControllersData
    {
        Controller<T> ResolveControllerByName(string s);

        string MainControllerName { get; }
    }
}