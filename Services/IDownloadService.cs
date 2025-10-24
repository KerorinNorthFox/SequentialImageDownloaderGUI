using MangaDownloader.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MangaDownloader.Services
{
    public interface IDownloadService
    {
        Task DownloadAll(IEnumerable<Manga> mangaList);

        Task Download(Manga manga);
    }
}
