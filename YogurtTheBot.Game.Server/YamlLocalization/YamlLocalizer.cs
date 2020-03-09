using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YogurtTheBot.Game.Core.Localizations;

namespace YogurtTheBot.Game.Server.YamlLocalization
{
    public class YamlLocalizer : ILocalizer
    {
        private readonly Func<string[], Localization> _localizationFactory;
        private readonly string _resourcesDirectory;
        private readonly string _defaultLanguage;

        public YamlLocalizer(LocalizationOptions localizationOptions, Func<string[], Localization> localizationFactory)
        {
            _localizationFactory = localizationFactory;
            _resourcesDirectory = localizationOptions.ResourcesDirectory;
            _defaultLanguage = localizationOptions.DefaultLanguage;
        }

        public Localization GetString(string key, string locale)
        {
            string[] format = GetFormat(key, locale);

            return _localizationFactory(format);
        }

        private string[] GetFormat(string key, string locale)
        {
            string[] keys = key.Split(".");

            string prePath = string.Join(
                Path.DirectorySeparatorChar,
                keys.Take(keys.Length - 1)
            );

            string[] pathToLookUp =
            {
                prePath + $".{locale}.yml",
                prePath + $".{_defaultLanguage}.yml",
                prePath + ".yml"
            };

            foreach (string path in pathToLookUp)
            {
                string finalPath = Path.Join(_resourcesDirectory, path);

                if (!File.Exists(finalPath)) continue;

                YamlMappingNode yaml = ParseFile(finalPath);
                string lastKey = keys.Last();

                foreach ((YamlNode yamlKey, YamlNode value) in yaml.Children)
                {
                    if ((yamlKey as YamlScalarNode)?.Value != lastKey) continue;

                    switch (value)
                    {
                        case YamlScalarNode scalarNode:
                            return new[] {scalarNode.Value};
                        
                        case YamlSequenceNode sequenceNode:
                            return sequenceNode
                                .Children
                                .Select(e => (e as YamlScalarNode)?.Value)
                                .Where(v => v != null)
                                .ToArray();
                    }
                }
            }

            return new[] {key};
        }

        private YamlMappingNode ParseFile(string path)
        {
            var yaml = new YamlStream();
            using var stringReader = new StreamReader(path);
            yaml.Load(stringReader);

            return (YamlMappingNode) yaml.Documents[0].RootNode;
        }
    }
}