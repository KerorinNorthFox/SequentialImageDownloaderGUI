using MangaDownloader.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MangaDownloader.Services
{
    public interface IDownloadService
    {
        Task DownloadAllAsync(IEnumerable<Manga> mangaList);

        Task DownloadAsync(Manga manga);
    }
}
