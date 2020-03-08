using System;
using System.Collections.Generic;
using System.Linq;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Handlers;
using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Language.Controllers
{
    public class LanguageController<T> : Controller<T> where T : IControllersData
    {
        public LanguageController(ILocalizer localizer) : base(localizer)
        {
            LanguageHandlers = (
                    from methodInfo in GetType().GetMethods()
                    let attribute =
                        Attribute.GetCustomAttribute(
                            methodInfo,
                            typeof(LanguageActionAttribute)
                        ) as LanguageActionAttribute
                    where !(attribute is null)
                    select new LanguageActionHandler<T>(
                        GetType().GetField(attribute.ExpressionMemberName).GetValue(this) as Expression,
                        localizer,
                        methodInfo
                    )
                )
                .Cast<IMessageHandler<T>>()
                .ToArray();
        }

        protected override IEnumerable<IMessageHandler<T>> GetHandlers()
        {
            return base.GetHandlers().Concat(LanguageHandlers);
        }

        public virtual IEnumerable<IMessageHandler<T>> LanguageHandlers { get; }
    }
}