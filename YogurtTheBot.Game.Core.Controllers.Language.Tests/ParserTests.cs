using System.Collections.Generic;
using System.Linq;
using Xunit;
using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;
using YogurtTheBot.Game.Core.Localizations;

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

        [Theory]
        [InlineData("Hello, Yegor")]
        [InlineData("Greetings, Yegor")]
        [InlineData("Hi Yegor")]
        public void TestLocalizedParser(string expression)
        {
            var hello = new LocalizedTerminal("hello");
            Expression name = !Alpha;
            Expression rule = hello + ~",".AsTerm() + !" ".AsTerm() + name + Expression.End;

            var parsingContext = new ParsingContext(expression)
            {
                Locale = "ru",
                Localizer = new DumbLocalizer(new Dictionary<string, string[]>
                {
                    {"hello", new[]
                    {
                        "Hello", "Hi", "Greetings"
                    }}
                })
            };

            ParsingResult? result = rule.TryParse(parsingContext);

            Assert.NotNull(result);
        }

        public readonly Expression Digit = "0".AsTerm() | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";

        public readonly Expression Alpha =
            new OneOf(
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".Select(c => c.ToString().AsTerm())
            );
    }
}