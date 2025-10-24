using MangaDownloader.Models;
using System;
using System.Collections.Generic;

namespace MangaDownloader.Services
{
    public interface IDownloadService
    {
        void DownloadAll(IList<Manga> mangaList);

        void Download(Manga manga);
    }
}
