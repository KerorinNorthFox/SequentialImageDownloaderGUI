using System.Collections.Generic;

namespace MangaDownloader.Desktop.Models
{
    public interface ISelectorProvider : IDictionary<string, SelectorMember>
    {
        void Load();

        void Reload();
    }
}