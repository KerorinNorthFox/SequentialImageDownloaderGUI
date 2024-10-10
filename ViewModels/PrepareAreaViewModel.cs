using MangaDownloader.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MangaDownloader.ViewModels
{
    public partial class PrepareAreaViewModel : ViewModelBase
    {
        public InputUrlViewModel InputUrlViewModel { get; }

        public UrlListViewModel UrlListViewModel { get; }

        private ImageDownloader _imageDownloader = new ImageDownloader();

        private ProgressEventSource _progress;

        public PrepareAreaViewModel(ProgressEventSource source)
        {
            UrlListViewModel = new UrlListViewModel();
            InputUrlViewModel = new InputUrlViewModel(UrlListViewModel);

            StartDownloadingCommand = ReactiveCommand.CreateFromTask(_startDownloading);

            _progress = source;
        }

        private bool _isDownloading = false;

        public bool IsDownloading
        {
            get => _isDownloading;
            set => this.RaiseAndSetIfChanged(ref _isDownloading, value);  
        }

        public ICommand StartDownloadingCommand { get; }

        private async Task _startDownloading()
        {
            _changeDownloadState();

            var urlList = UrlListViewModel.UrlList;
            if (urlList.Count <= 0)
            {
                return;
            }

            List<List<Uri>> imageUrisList = new List<List<Uri>>(); // ダウンロードする画像の二次元リスト
            List<int> failedPageUriIndex = new List<int>(); // DL失敗したページのインデックス

            int uriCount = 0;
            int currentPageUriIndex = 0;
            try
            {
                _progress.OnSetMaxProgress(urlList.Count);
                foreach (var url in urlList)
                {
                    var imageUris = await _imageDownloader.GetSequentialImagesUris(url);
                    imageUrisList.Add(imageUris);

                    uriCount += imageUris.Count();
                    currentPageUriIndex++;
                    _progress.OnUpdateProgress(1);
                }
                _progress.OnResetProgress();
                _progress.OnSetMaxProgress(uriCount);

                currentPageUriIndex = 0;
                foreach (var imageUris in imageUrisList)
                {
                    // TODO:configから画像保存先を設定できるようにする
                    var baseDir = @"C:\Images";
                    var saveDirPath = ImageDownloader.GenerateSaveDirPathFromUri(baseDir, urlList[currentPageUriIndex]);
                    ImageDownloader.CreateSaveDir(saveDirPath);

                    int imageIndex = 0;
                    await foreach (var image in _imageDownloader.DownloadImages(imageUris))
                    {
                        // 画像保存ボタンを別に作ってそこでディレクトリ作成してもいいかも？
                        // ・ページネーションを実装してUrlごとに画像表示領域を作る
                        // ・画像はチェックボックスで保存に含めるか
                        var imagePath = Path.Combine(saveDirPath, $"{imageIndex}.jpg");
                        imageIndex++;
                        _progress.OnUpdateProgress(1);

                        if (File.Exists(imagePath))
                        {
                            continue;
                        }
                        image.Save(imagePath);
                    }
                    currentPageUriIndex++;
                }
            }
            catch (FailedDownloadException)
            {
                failedPageUriIndex.Add(currentPageUriIndex);
            }

            UrlListViewModel.ClearUrlList(failedPageUriIndex);
            _changeDownloadState();
        }

        private void _changeDownloadState()
        {
            IsDownloading = !IsDownloading;
        }
    }
}
