using YogurtTheBot.Game.Core.Controllers.Language.Expressions;

namespace YogurtTheBot.Game.Core.Controllers.Language
{
    public static class Extensions
    {
        public static Terminal AsTerm(this string s)
        {
            return new Terminal(s);
        }
    }
}