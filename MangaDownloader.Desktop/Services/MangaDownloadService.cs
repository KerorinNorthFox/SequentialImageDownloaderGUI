using AngleSharp.Dom;
using MangaDownloader.Desktop.Models;
using MangaDownloader.Desktop.Models.Events;
using MangaDownloader.Rule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MangaDownloader.Desktop.Services
{
    public class MangaDownloadService : IDownloadService
    {
        private ImageDownloader _imageDownloader = new ImageDownloader();

        private IProgressEvents _mangaProgress;

        private IRuleProvider _rules;

        private readonly int _maxConcurrentMangaDownloads = 10;

        private readonly SemaphoreSlim _mangaDownloadSemaphore;

        /// <summary>
        /// Pagesのimageの並列ダウンロード数制限
        /// </summary>
        private readonly int _maxConcurrentImageDownloads = 10;

        private readonly SemaphoreSlim _imageDownloadSemaphore;

        private readonly Config _config;

        public MangaDownloadService(IRuleProvider rules, IProgressEvents mangaProgress, Config config)
        {
            _rules = rules;
            _mangaProgress = mangaProgress;
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
                        Debug.WriteLine(e.Message);
                        // TODO: 失敗したことを伝えるメッセージボックスを表示.
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
            manga.ChangeDownloadState(DownloadStatus.Downloading);

            var rule = matchRule(manga.Uri.Host);
            if (rule == null)
            {
                throw new FailedDownloadException($"Rule is not found. Requires '{manga.Uri.Host}.dll' plugin.");
            }

            IDocument doc;
            try
            {
                doc = await rule.GetDocument(manga.Uri);
            }
            catch (Exception e)
            {
                throw new FailedDownloadException($"Getting html document failed! >> {e.Message}");
            }

            IEnumerable<Uri> imageUris;
            try
            {
                imageUris = rule.ParsePageUri(doc, manga.Uri.Segments[^1]);
            }
            catch (Exception e)
            {
                throw new FailedDownloadException($"Parse html failed! Fix selectors and replace {rule.Selector.Domain}.dll plugin. >> {e.Message}");
            }
            var title = rule.GetTitle(doc);
            var author = rule.GetAuthor(doc);

            var imageDownloadTasks = imageUris.Select(async (uri, index) =>
            {
                await _imageDownloadSemaphore.WaitAsync();
                try
                {
                    var image = await _imageDownloader.Download(uri);
                    return new { Index = index, Uri = uri, Image = image };
                }
                catch (Exception e)
                {
                    throw new FailedDownloadException(e.Message);
                }
                finally
                {
                    _imageDownloadSemaphore.Release();
                }
            });

            var results = await Task.WhenAll(imageDownloadTasks);
            foreach (var result in results)
            {
                manga.AddPageByIndex(result.Index, result.Uri, result.Image);
            }

            manga.SaveDir = rule.BuildSaveDirPath(_config.SaveBasePath, manga.Uri, title, author);
            Debug.WriteLine(manga.SaveDir);

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
