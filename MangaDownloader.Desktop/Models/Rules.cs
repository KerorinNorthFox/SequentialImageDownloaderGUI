using MangaDownloader.Desktop.Activator;
using MangaDownloader.Rule;
using System.Collections.Generic;

namespace MangaDownloader.Desktop.Models
{
    public class Rules : Dictionary<string, IRule>, IRuleProvider
    {
        private string _pluginPath;

        public Rules(string pluginPath)
        {
            _pluginPath = pluginPath;

            Load();
        }

        public void Load()
        {
            load();
        }

        private void load()
        {
            var rules = RuleActivator.LoadRules(_pluginPath);
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
