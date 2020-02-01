using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using StrategyBot.Game.Logic;
using YamlDotNet.Serialization;

namespace StrategyBot.Game.Server
{
    public class YamlLocalizer : ILocalizer
    {
        private readonly string _resourcesDirectory;
        private readonly string _defaultLanguage;
        private readonly Deserializer _deserializer;

        public YamlLocalizer(LocalizationOptions localizationOptions)
        {
            _resourcesDirectory = localizationOptions.ResourcesDirectory;
            _defaultLanguage = localizationOptions.DefaultLanguage;
            _deserializer = new Deserializer();
        }

        public string GetString(string key, string locale, params object[] args)
        {
            string format = GetFormat(key, locale);

            return string.Format(format, args);
        }

        private string GetFormat(string key, string locale)
        {
            string[] keys = key.Split(".");
            
            string prePath = string.Join(
                Path.DirectorySeparatorChar, 
                keys.Take(keys.Length - 1)
            );
            
            string[] pathToLookUp = {
                prePath + $".{locale}.yml",
                prePath + $".{_defaultLanguage}.yml",
                prePath + ".yml"
            };
            
            foreach (string path in pathToLookUp)
            {
                string finalPath = Path.Join(_resourcesDirectory, path);

                if (!File.Exists(finalPath)) continue;
                
                Dictionary<string, string> yaml = ParseFile(finalPath);

                if (yaml.TryGetValue(keys.Last(), out string res))
                {
                    return res;
                }
            }

            return key;
        }

        private Dictionary<string, string> ParseFile(string path)
        {
            string content = File.ReadAllText(path);
            return _deserializer.Deserialize<Dictionary<string, string>>(content);
        }
    }
}