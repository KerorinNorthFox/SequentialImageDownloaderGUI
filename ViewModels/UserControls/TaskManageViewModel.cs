using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MangaDownloader.Models;
using MangaDownloader.Models.EventSources;
using ReactiveUI;

namespace MangaDownloader.ViewModels
{
    public partial class TaskManageViewModel : ViewModelBase
    {
        public InputUrlViewModel InputUrlViewModel { get; }

        public MangaListViewModel MangaListViewModel { get; } = new MangaListViewModel();

        private ImageDownloader _imageDownloader = new ImageDownloader();

        // 親からダウンロード開始時のメソッドをコマンドとして貰う
        public TaskManageViewModel()
        {
            InputUrlViewModel = new InputUrlViewModel(
                new InputUrlEventSource().Subscribe(MangaListViewModel.AddManga)
                );

            StartDownloadCommand = ReactiveCommand.CreateFromTask(startDownload);
            UploadDiscordCommand = ReactiveCommand.Create(uploadDiscordCommand);
        }

        public ICommand StartDownloadCommand { get; }

        public ICommand UploadDiscordCommand { get; }

        private async Task startDownload()
        {
            var mangaList = MangaListViewModel.MangaList;
            if (mangaList.Count <= 0)
            {
                return;
            }

            for (int i = 0; i <= MangaListViewModel.MangaList.Count; i++)
            {
                startMangaDownload(MangaListViewModel.MangaList[i]);
            }
        }

        private async Task startMangaDownload(Manga manga)
        {
            try
            {
                manga.ChangeDownloadState(DownloadState.Downloading);

                var imageUris = await _imageDownloader.GetSequentialImagesUris(manga.MangaUri);
            }
            catch (Exception e) { }
        }

        private void uploadDiscordCommand()
        {

        }
    }
}

// メモ
// addボタンでurlを追加したらMangaTaskクラスのインスタンス作成して、"MangaTasks"リストに追加
// UrlListViewModelのUrlListには"MangaTasks"リストから要素を縦に並べる
// 並べる際に、コンバーターでMangaTaskインスタンスからタイトル(ない場合はURL)を表示するようにする