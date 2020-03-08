using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using Expression = System.Linq.Expressions.Expression;

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