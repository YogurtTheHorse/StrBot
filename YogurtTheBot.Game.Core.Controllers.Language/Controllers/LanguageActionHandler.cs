using System;
using System.Reflection;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Handlers;
using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using YogurtTheBot.Game.Core.Controllers.Language.Parsing;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Language.Controllers
{
    public class LanguageActionHandler<T> : BaseMessageHandler<T> where T : IControllersData
    {
        private readonly Expression _expression;
        private readonly ILocalizer _localizer;

        public LanguageActionHandler(Expression expression, ILocalizer localizer, MethodInfo methodInfo)
            : base(methodInfo)
        {
            _expression = expression;
            _localizer = localizer;
        }

        public override bool CanHandle(IncomingMessage message, PlayerInfo playerInfo)
        {
            var parsingContext = new ParsingContext(message.Text)
            {
                Locale = playerInfo.Locale,
                Localizer = _localizer
            };

            return _expression.TryParse(parsingContext) != null;
        }
    }
}