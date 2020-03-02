using System;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Handlers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : Attribute, IHandlerAttribute
    {
        public LocalizationPath LocalizationPath { get; }

        public ActionAttribute(LocalizationPath localizationPath)
        {
            LocalizationPath = localizationPath;
        }

        public ActionAttribute(string localizationPath) : this((LocalizationPath) localizationPath)
        {
        }
    }
}