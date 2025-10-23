using Avalonia.Media.Imaging;
using MangaDownloader.Models;
using MangaDownloader.Models.Config;
using ReactiveUI;
using System;
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

        // 親からダウンロード開始時のメソッドをコマンドとして貰う(?なにこれ)
        public TaskManageViewModel(Config config)
        {
            _config = config;

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

            for (int i = 0; i <= mangaList.Count; i++)
            {
                try
                {
                    var manga = mangaList[i]; // mangaは参照を持つので、値を変更してもMangaListViewModel.MangaListに反映される.
                    manga.ChangeDownloadState(DownloadState.Downloading);

                    var doc = await _downloader.GetDocument(manga.Uri);
                    var imageUris = _downloader.ParsePageUri(doc, manga.Uri);

                    foreach (var imageUri in imageUris)
                    {
                        Bitmap image = await _downloader.DownloadImage(imageUri);
                        manga.AddPageByIndex(i, imageUri, image); //TODO
                    }

                    manga.ChangeDownloadState(DownloadState.Finished); //TODO
                }
                catch (FailedDownloadException e)
                {

                }
            }

            IsDownloading = false;
        }
    }
}

// メモ
// addボタンでurlを追加したらMangaTaskクラスのインスタンス作成して、"MangaTasks"リストに追加
// UrlListViewModelのUrlListには"MangaTasks"リストから要素を縦に並べる
// 並べる際に、コンバーターでMangaTaskインスタンスからタイトル(ない場合はURL)を表示するようにする