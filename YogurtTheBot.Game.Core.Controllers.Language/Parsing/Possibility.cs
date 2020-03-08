using YogurtTheBot.Game.Core.Controllers.Language.Nodes;

namespace YogurtTheBot.Game.Core.Controllers.Language.Parsing
{
    public class Possibility
    {
        public ParsingContext Context { get; set; }
        
        public INode Node { get; set; }
    }
}