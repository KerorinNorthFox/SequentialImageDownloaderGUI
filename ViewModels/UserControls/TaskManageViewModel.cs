using MangaDownloader.Models;
using MangaDownloader.Models.Config;
using ReactiveUI;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MangaDownloader.ViewModels
{
    public partial class TaskManageViewModel : ViewModelBase
    {
        public InputUrlViewModel InputUrlViewModel { get; }

        public MangaListViewModel MangaListViewModel { get; } = new MangaListViewModel();

        private ImageDownloader _downloader;

        private Config _config;

        private bool _isDownloading = false;

        /// <summary>
        /// 並列ダウンロード数の上限
        /// </summary>
        private readonly int _maxConcurrentDownloads = 10;

        private readonly SemaphoreSlim _downloadSemaphore;

        public TaskManageViewModel(Config config)
        {
            _config = config;
            _downloader = new ImageDownloader(_config.SelectorJsonPath);
            _downloadSemaphore = new SemaphoreSlim(_maxConcurrentDownloads, _maxConcurrentDownloads);
            InputUrlViewModel = new InputUrlViewModel(MangaListViewModel.AddManga);

            var canStartDownload = this.WhenAnyValue(x => x.IsDownloading, isDownloading => !isDownloading);
            StartDownloadCommand = ReactiveCommand.CreateFromTask(startDownload, canStartDownload);

            this.WhenAnyValue(x => x.IsDownloading)
                .Subscribe(isDownloading =>
                {
                    InputUrlViewModel.IsDownloading = isDownloading;
                    MangaListViewModel.IsDownloading = isDownloading;
                });
        }

        public bool IsDownloading
        {
            get => _isDownloading;
            set => this.RaiseAndSetIfChanged(ref _isDownloading, value);
        }

        public ICommand StartDownloadCommand { get; }

        private async Task startDownload()
        {
            if (IsDownloading)
            {
                return;
            }

            var mangaList = MangaListViewModel.MangaList;
            if (mangaList.Count <= 0)
            {
                return;
            }

            IsDownloading = true;

            for (int i = 0; i < mangaList.Count; i++)
            {
                try
                {
                    var manga = mangaList[i]; // mangaは参照を持つので、値を変更してもMangaListViewModel.MangaListに反映される.
                    if (manga.State != DownloadStatus.Pending || manga.State != DownloadStatus.Failed)
                    {
                        continue;
                    }
                    manga.ChangeDownloadState(DownloadStatus.Downloading);

                    var doc = await _downloader.GetDocument(manga.Uri);
                    var imageUris = _downloader.ParsePageUri(doc, manga.Uri);
                    var imageDownloadTasks = imageUris.Select(async (uri, index) =>
                    {
                        await _downloadSemaphore.WaitAsync(); // セマフォで並列数を制限
                        try
                        {
                            var image = await _downloader.DownloadImage(uri);
                            return new { Index = index, Uri = uri, Image = image };
                        }
                        finally
                        {
                            _downloadSemaphore.Release(); // 完了後にセマフォを解放
                        }
                    });
                    var results = await Task.WhenAll(imageDownloadTasks);
                    foreach (var result in results)
                    {
                        manga.AddPageByIndex(result.Index, result.Uri, result.Image);
                    }

                    manga.ChangeDownloadState(DownloadStatus.Finished); //TODO
                }
                catch (FailedDownloadException e)
                {
                }
            }

            IsDownloading = false;
        }
    }
}
