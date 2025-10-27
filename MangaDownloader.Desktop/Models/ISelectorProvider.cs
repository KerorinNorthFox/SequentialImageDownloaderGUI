using System.Collections.Generic;

namespace MangaDownloader.Models
{
    public interface ISelectorProvider : IDictionary<string, SelectorMember>
    {
        void Load();

        void Reload();
    }
}