using System;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Handlers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ActionAttribute : Attribute
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