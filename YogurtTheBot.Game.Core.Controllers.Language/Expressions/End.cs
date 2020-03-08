using YogurtTheBot.Game.Core.Controllers.Language.Parsing;

namespace YogurtTheBot.Game.Core.Controllers.Language.Expressions
{
    public class End : Terminal
    {
        public End() : base(string.Empty)
        {
        }

        public override ParsingResult? TryParse(ParsingContext ctx)
        {
            return ctx.Position == ctx.Text.Length 
                ? base.TryParse(ctx) 
                : null;
        }
    }
}