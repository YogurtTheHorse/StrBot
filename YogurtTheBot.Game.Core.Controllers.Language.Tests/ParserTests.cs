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
            Expression rule = !Digit + Expression.End;

            ParsingResult? result = rule.TryParse(number.ToString());

            Assert.NotNull(result);
            Assert.Single(result.Possibilities);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1+1")]
        [InlineData("3+1")]
        [InlineData("5/3-2")]
        public void SimpleMath(string expression)
        {
            Expression number = new NonTerminal("digit")
            {
                Rule = !Digit
            };
            Expression op = new NonTerminal("op")
            {
                Rule = "+".AsTerm() | "-" | "*" | "/"
            };
            var mathExpression = new NonTerminal("expr");
            mathExpression.Rule = number | (number + op + mathExpression);
            Expression rule = mathExpression + Expression.End;

            ParsingResult? result = rule.TryParse(expression);

            Assert.NotNull(result);
        }

        public Expression Digit = "0".AsTerm() | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";
    }
}