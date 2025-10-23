using MangaDownloader.Models;
using MangaDownloader.Models.Config;
using ReactiveUI;
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

        /// <summary>
        /// ダウンロード中かどうかのフラグ
        /// </summary>
        public bool IsDownloading = false;

        /// <summary>
        /// 並列ダウンロード数の上限
        /// </summary>
        private readonly int _maxConcurrentDownloads = 10;

        private readonly SemaphoreSlim _downloadSemaphore;

        public TaskManageViewModel(Config config)
        {
            _config = config;
            _downloadSemaphore = new SemaphoreSlim(_maxConcurrentDownloads, _maxConcurrentDownloads);

            _downloader = new ImageDownloader(_config.SelectorJsonPath);
            InputUrlViewModel = new InputUrlViewModel(MangaListViewModel.AddManga);

            StartDownloadCommand = ReactiveCommand.CreateFromTask(startDownload);
        }

        public ICommand StartDownloadCommand { get; }


        private async Task startDownload()
        {
            // TODO: IsDownloadフラグでダウンロードボタンとAddUriボタンを無効化, dl終了までにmangaListに変更を加えないようにする
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
