using MangaDownloader.Desktop.Activator;
using MangaDownloader.Rule;
using System.Collections.Generic;

namespace MangaDownloader.Desktop.Models
{
    public class Rules : Dictionary<string, IRule>, IRuleProvider
    {
        private Config _config;

        public Rules(Config config)
        {
            _config = config;

            Load();
        }

        public void Load()
        {
            load();
        }

        private void load()
        {
            var rules = RuleActivator.LoadRules(_config.PluginPath);
            foreach (var rule in rules)
            {
                this.Add(rule.Selector.Domain, rule);
            }
        }

        public void Reload()
        {
            Load();
        }
    }
}
