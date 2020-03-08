using Xunit;
using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(54)]
        [InlineData(100)]
        public void TestNumbers(int number)
        {
            Expression rule = !("0".AsTerm() | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9") + Expression.End;

            ParsingResult? result = rule.TryParse(number.ToString());

            Assert.NotNull(result);
            Assert.Single(result.Possibilities);
        }
    }
}