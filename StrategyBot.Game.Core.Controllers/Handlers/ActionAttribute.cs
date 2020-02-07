using System;
using StrategyBot.Game.Core.Localizations;

namespace StrategyBot.Game.Core.Controllers.Handlers
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