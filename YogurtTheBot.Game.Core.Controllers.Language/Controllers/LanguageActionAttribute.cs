using System;

namespace YogurtTheBot.Game.Core.Controllers.Language.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LanguageActionAttribute : Attribute
    {
        public string ExpressionMemberName { get; }

        public LanguageActionAttribute(string expressionMemberName)
        {
            ExpressionMemberName = expressionMemberName;
        }
    }
}