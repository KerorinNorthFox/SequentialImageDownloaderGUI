using MangaDownloader.Rule;
using System.Collections.Generic;

namespace MangaDownloader.Desktop.Models
{
    public interface IRuleProvider : IDictionary<string, IRule>
    {
        void Load();

        void Reload();
    }
}