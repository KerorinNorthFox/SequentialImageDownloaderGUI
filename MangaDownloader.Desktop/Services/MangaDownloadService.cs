using MangaDownloader.Desktop.Models;
using MangaDownloader.Desktop.Models.Events;
using MangaDownloader.Rule;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MangaDownloader.Desktop.Services
{
    public class MangaDownloadService : IDownloadService
    {
        private IImageDownloader _downloader;

        private IProgressEvents _mangaProgress;

        private IProgressEvents _imageProgress;

        private IRuleProvider _rules;

        private readonly int _maxConcurrentMangaDownloads = 10;

        private readonly SemaphoreSlim _mangaDownloadSemaphore;

        /// <summary>
        /// Pagesのimageの並列ダウンロード数制限
        /// </summary>
        private readonly int _maxConcurrentImageDownloads = 10;

        private readonly SemaphoreSlim _imageDownloadSemaphore;

        private readonly Config _config;

        public MangaDownloadService(IImageDownloader downloader, IRuleProvider rules, IProgressEvents mangaProgress, IProgressEvents imageProgress, Config config)
        {
            _downloader = downloader;
            _rules = rules;
            _mangaProgress = mangaProgress;
            _imageProgress = imageProgress;
            _config = config;
            _mangaDownloadSemaphore = new SemaphoreSlim(_maxConcurrentMangaDownloads, _maxConcurrentMangaDownloads);
            _imageDownloadSemaphore = new SemaphoreSlim(_maxConcurrentImageDownloads, _maxConcurrentImageDownloads);
        }

        public async Task DownloadAllAsync(IEnumerable<Manga> mangaList)
        {
            _mangaProgress.OnInitializeProgress(mangaList.ToList().Count);
            var downloadTasks = mangaList
                .Where(m => m.State == DownloadStatus.Pending || m.State == DownloadStatus.Failed)
                .Select(async manga =>
                {
                    await _mangaDownloadSemaphore.WaitAsync();
                    try
                    {
                        await DownloadAsync(manga);
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

        public async Task DownloadAsync(Manga manga)
        {
            var rule = matchRule(manga.Uri.Host);
            if (rule == null)
            {
                manga.ChangeDownloadState(DownloadStatus.Failed);
                return;
            }

            manga.ChangeDownloadState(DownloadStatus.Downloading);

            var doc = await rule.GetDocument(manga.Uri);
            var imageUris = rule.ParsePageUri(doc, manga.Uri.Segments[^1]);
            var title = rule.GetTitle(doc);
            var author = rule.GetAuthor(doc);
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

            manga.SaveDir = rule.BuildSaveDirPath(_config.SaveBasePath, manga.Uri, title, author);

            manga.ChangeDownloadState(DownloadStatus.Finished);
        }

        private IRule? matchRule(string host)
        {
            if (_rules.TryGetValue(host, out var rule))
            {
                return rule;
            }
            else
            {
                return null;
            }
        }
    }
}
