using System;
using System.Linq;
using System.Reflection;
using YogurtTheBot.Game.Core.Controllers.Answers;
using YogurtTheBot.Game.Core.Controllers.Handlers;
using YogurtTheBot.Game.Data;

namespace YogurtTheBot.Game.Core.Controllers.Abstractions
{
    public abstract class ControllerBase : IController
    {
        protected ControllerBase()
        {
            // TODO: refactor
            ActionsInfos = GetType()
                .GetMethods()
                .Where(m => Attribute.IsDefined(m, typeof(ActionAttribute)))
                .Select(m => (m, (ActionAttribute)Attribute.GetCustomAttribute(m, typeof(ActionAttribute))))
                .ToArray();
            
            GreetingInfo = GetType()
                .GetMethods()
                .Where(m => Attribute.IsDefined(m, typeof(GreetingsAttribute)))
                .Select(m => ((MethodInfo, GreetingsAttribute)?)(m, (GreetingsAttribute)Attribute.GetCustomAttribute(m, typeof(GreetingsAttribute))))
                .DefaultIfEmpty(null)
                .FirstOrDefault();
            
            DefaultInfo = GetType()
                .GetMethods()
                .Where(m => Attribute.IsDefined(m, typeof(DefaultAttribute)))
                .Select(m => ((MethodInfo, DefaultAttribute)?)(m, (DefaultAttribute)Attribute.GetCustomAttribute(m, typeof(DefaultAttribute))))
                .DefaultIfEmpty(null)
                .FirstOrDefault();
        }

        protected virtual IControllerAnswer Answer(string text) =>
            new ControllerAnswer
            {
                Suggestions = GetSuggestions(),
                Text = text
            };

        protected virtual Suggestion[] GetSuggestions()
        {
            return Array.Empty<Suggestion>();
        }

        public (MethodInfo methodInfo, ActionAttribute actionAttribute)[] ActionsInfos { get; }
        public (MethodInfo methodInfo, GreetingsAttribute greetingsAttribute)? GreetingInfo { get; }
        
        public (MethodInfo methodInfo, DefaultAttribute defaultAttribute)? DefaultInfo { get; }
    }
}
