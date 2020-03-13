namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public interface IControllersProvider<T> where T : IControllersData
    {
        ControllerBase<T> ResolveControllerByName(string s);

        string MainControllerName { get; }
    }
}