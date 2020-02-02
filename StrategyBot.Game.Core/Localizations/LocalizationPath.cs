using System;
using System.Dynamic;
using StrategyBot.Game.Logic.Helpers;

namespace StrategyBot.Game.Logic.Localizations
{
    public class LocalizationPath : DynamicObject
    {
        private readonly string _path;

        protected LocalizationPath(string path)
        {
            _path = path;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string prefix = _path.Length > 0
                ? $"{_path}."
                : string.Empty;
            
            result = new LocalizationPath(prefix + binder.Name.ToSnakeCase());
            
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            if (args.Length == 0)
            {
                result = new LocalizationDescription(_path);
                return true;
            }

            if (args.Length != 1 || !(args[0] is Func<PlayerState, PlayerData, object[]> argsFactory))
            {
                // throw new InvalidOperationException(
                //     "LocalizationPath object should be invoked with only one argument of type " +
                //     "Func<PlayerState, PlayerData, object[]>."
                // );
                result = null;
                return false;
            }

            result = new LocalizationDescription(_path, argsFactory);
            return true;
        }

        public static dynamic Root => new LocalizationPath(string.Empty);
    }
}