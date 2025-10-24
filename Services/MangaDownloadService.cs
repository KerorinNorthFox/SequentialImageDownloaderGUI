using MangaDownloader.Models;
using MangaDownloader.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MangaDownloader.Services
{
    public class MangaDownloadService : IDownloadService
    {
        private IImageDownloader _downloader;

        private IProgressEvents _mangaProgress;

        private IProgressEvents _imageProgress;

        private readonly int _maxConcurrentMangaDownloads = 10;

        private readonly SemaphoreSlim _mangaDownloadSemaphore;

        /// <summary>
        /// Pagesのimageの並列ダウンロード数制限
        /// </summary>
        private readonly int _maxConcurrentImageDownloads = 10;

        private readonly SemaphoreSlim _imageDownloadSemaphore;

        public MangaDownloadService(IImageDownloader downloader, IProgressEvents mangaProgress, IProgressEvents imageProgress)
        {
            _downloader = downloader;
            _mangaProgress = mangaProgress;
            _imageProgress = imageProgress;
            _mangaDownloadSemaphore = new SemaphoreSlim(_maxConcurrentMangaDownloads, _maxConcurrentMangaDownloads);
            _imageDownloadSemaphore = new SemaphoreSlim(_maxConcurrentImageDownloads, _maxConcurrentImageDownloads);
        }

        public async Task DownloadAll(IEnumerable<Manga> mangaList)
        {
            var mangaCount = mangaList.ToList().Count;
            _mangaProgress.OnInitializeProgress(mangaCount);
            var downloadTasks = mangaList.Where(m => m.State == DownloadStatus.Pending || m.State == DownloadStatus.Failed)
                .Select(async manga =>
                {
                    await _mangaDownloadSemaphore.WaitAsync();
                    try
                    {
                        await Download(manga);
                    }
                    catch (FailedDownloadException e)
                    {
                        manga.ChangeDownloadState(DownloadStatus.Failed);
                    }
                    finally
                    {
                        _mangaDownloadSemaphore.Release();
                        _mangaProgress.OnUpdateProgress(1);
                    }
                });

            await Task.WhenAll(downloadTasks);
        }

        public async Task Download(Manga manga)
        {
            manga.ChangeDownloadState(DownloadStatus.Downloading);

            var doc = await _downloader.GetDocument(manga.Uri);
            var imageUris = _downloader.ParsePageUri(doc, manga.Uri);
            _imageProgress.OnInitializeProgress(imageUris.ToList().Count);
            var imageDownloadTasks = imageUris.Select(async (uri, index) =>
            {
                await _imageDownloadSemaphore.WaitAsync();
                try
                {
                    var image = await _downloader.DownloadImage(uri);
                    return new { Index = index, Uri = uri, Image = image };
                }
                finally
                {
                    _imageDownloadSemaphore.Release();
                    _imageProgress.OnUpdateProgress(1);
                }
            });
            var results = await Task.WhenAll(imageDownloadTasks);
            foreach (var result in results)
            {
                manga.AddPageByIndex(result.Index, result.Uri, result.Image);
            }

            manga.ChangeDownloadState(DownloadStatus.Finished);
        }

    }
}
