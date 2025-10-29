using MangaDownloader.Desktop.Activator;
using MangaDownloader.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloader.Desktop.Models
{
    public class Rules : Dictionary<string, IRule>, IRuleProvider
    {
        private RuleActivator _activator;

        private Config _config;

        public Rules(Config config)
        {
            _config = config;
            _activator = new RuleActivator(_config);

            Load();
        }

        public void Load()
        {
            load();
        }

        private void load()
        {

        }

        public void Reload()
        {
            Load();
        }
    }
}
