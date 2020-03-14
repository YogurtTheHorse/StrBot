using System.Reflection;
using YogurtTheBot.Game.Core.Communications;
using YogurtTheBot.Game.Core.Controllers.Abstractions;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Core.Controllers.Handlers
{
    public class ActionHandler<T> : BaseMessageHandler<T> where T : IControllersData
    {
        public LocalizationPath LocalizationPath { get; }

        private readonly ILocalizer _localizer;

        public ActionHandler(LocalizationPath localizationPath, ILocalizer localizer, MethodInfo methodInfo)
            : base(methodInfo)
        {
            LocalizationPath = localizationPath;
            _localizer = localizer;
        }

        public override CanHandleResult CanHandle(IncomingMessage message, PlayerInfo playerInfo)
        {
            Localization actionString = _localizer.GetString(LocalizationPath.Path, playerInfo.Locale);

            return new CanHandleResult
            {
                CanHandle = actionString.MatchesMessage(message.Text)
            };
        }
    }
}