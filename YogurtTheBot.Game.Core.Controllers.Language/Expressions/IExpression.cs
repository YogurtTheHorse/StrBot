using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public abstract class Expression
    {
        public static readonly Expression Empty = string.Empty.AsTerm();
        
        public static readonly Expression End = new End();
        
        public abstract ParsingResult? TryParse(ParsingContext parsingContext);

        public ParsingResult? TryParse(string s) => TryParse(new ParsingContext(s));

        public static Expression operator +(Expression e1, Expression e2) => new Sequence(e1, e2);

        public static Expression operator +(Expression e1, string e2) => new Sequence(e1, e2.AsTerm());

        public static Expression operator |(Expression e1, Expression e2) => new OneOf(new[] {e1, e2});

        public static Expression operator |(Expression e1, string e2) => e1 | e2.AsTerm();

        public static Expression operator ~(Expression e) => e | Empty;

        public static Expression operator !(Expression e) => new Many(e);
    }
}