using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Controllers.Handlers;
using YogurtTheBot.Game.Core.Controllers.Language.Expressions;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Language.Controllers
{
    public class LanguageController<T> : ActionController<T> where T : IControllersData
    {
        public LanguageController(IControllersProvider<T> controllersProvider, ILocalizer localizer)
            : base(controllersProvider, localizer)
        {
            var handlers = new List<IMessageHandler<T>>();

            foreach (MethodInfo methodInfo in GetType().GetMethods())
            {
                var attribute = Attribute.GetCustomAttribute(
                    methodInfo,
                    typeof(LanguageActionAttribute)
                ) as LanguageActionAttribute;

                if (attribute is null) continue;

                // ReSharper disable once UseNegatedPatternMatching
                var expression = GetType().GetProperty(attribute.ExpressionPropertyName)?.GetValue(this) as Expression;

                if (expression is null)
                {
                    throw new InvalidOperationException(
                        "Couldn't find expression property with name " + attribute.ExpressionPropertyName
                    );
                }

                handlers.Add(new LanguageActionHandler<T>(
                    expression, localizer, methodInfo)
                );
            }

            LanguageHandlers = handlers.ToArray();
        }

        protected override IEnumerable<IMessageHandler<T>> GetHandlers()
        {
            return base.GetHandlers().Concat(LanguageHandlers);
        }

        public virtual IEnumerable<IMessageHandler<T>> LanguageHandlers { get; }
    }
}