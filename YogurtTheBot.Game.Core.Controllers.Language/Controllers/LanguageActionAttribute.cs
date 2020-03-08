using System;

namespace YogurtTheBot.Game.Core.Controllers.Language.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LanguageActionAttribute : Attribute
    {
        public string ExpressionPropertyName { get; }

        public LanguageActionAttribute(string expressionPropertyName)
        {
            ExpressionPropertyName = expressionPropertyName;
        }
    }
}