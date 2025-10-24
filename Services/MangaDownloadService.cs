using MangaDownloader.Models;
using MangaDownloader.Models.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MangaDownloader.Services
{
    public class MangaDownloadService : IDownloadService
    {
        private IProgressEvents _progress;

        /// <summary>
        /// Mangaの並列ダウンロード数制限
        /// </summary>
        private readonly int _maxConcurrentMangaDownloads = 10;

        /// <summary>
        /// Pagesのimageの並列ダウンロード数制限
        /// </summary>
        private readonly int _maxConcurrentImageDownloads = 10;

        private readonly SemaphoreSlim _mangaDownloadSemaphore;

        private readonly SemaphoreSlim _imageDownloadSemaphore;

        public MangaDownloadService(IProgressEvents progress)
        {
            _progress = progress;
            _mangaDownloadSemaphore = new SemaphoreSlim(_maxConcurrentMangaDownloads, _maxConcurrentMangaDownloads);
            _imageDownloadSemaphore = new SemaphoreSlim(_maxConcurrentImageDownloads, _maxConcurrentImageDownloads);
        }

        public void DownloadAll(IList<Manga> mangaList)
        {
            for (int i = 0; i < mangaList.Count; i++)
            {
                try
                {
                    var manga = mangaList[i];
                }
            }
        }

        public void Download(Manga manga)
        {

        }
    }
}
