using MangaDownloader.Desktop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MangaDownloader.Desktop.Services
{
    public interface IDownloadService
    {
        Task DownloadAllAsync(IEnumerable<Manga> mangaList);

        Task DownloadAsync(Manga manga);
    }
}
