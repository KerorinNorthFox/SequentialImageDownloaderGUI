using Avalonia.Controls;
using ReactiveUI;
using System;

namespace MangaDownloader.ViewModels
{
    public partial class DownloadingInfoViewModel : ViewModelBase
    {
        public DownloadingInfoViewModel()
        {
            if (Design.IsDesignMode)
            {
                DownloadingUrl = "https://examplewoooooooooooo.com";
            }
        }

        private int _totalUrlNumber;

        /// <summary>
        /// ダウンロードプロセス中のURLの総数
        /// </summary>
        public int TotalUrlNumber
        {
            get => _totalUrlNumber;
            set => this.RaiseAndSetIfChanged(ref _totalUrlNumber, value);
        }

        private int _currentUrlIndex;

        /// <summary>
        /// 現在ダウンロードしているURLの番号
        /// </summary>
        public int CurrentUrlIndex
        {
            get => _currentUrlIndex;
            set => this.RaiseAndSetIfChanged(ref _currentUrlIndex, value);
        }

        private string _downloadingUrl = string.Empty;

        /// <summary>
        /// 現在ダウンロードしているページのURL
        /// </summary>
        public string DownloadingUrl
        {
            get => _downloadingUrl;
            set => this.RaiseAndSetIfChanged(ref _downloadingUrl, value);
        }

        private int _totalImageUrlNumber;

        /// <summary>
        /// ダウンロード中の連番画像総数
        /// </summary>
        public int TotalImageUrlNumber
        {
            get => _totalImageUrlNumber;
            set => this.RaiseAndSetIfChanged(ref _totalImageUrlNumber, value);
        }

        private int _currentImageUrlIndex;

        /// <summary>
        /// 現在ダウンロードしている連番画像の番号
        /// </summary>
        public int CurrentImageUrlIndex
        {
            get => _currentImageUrlIndex;
            set => this.RaiseAndSetIfChanged(ref _currentImageUrlIndex, value);
        }


        private void _reset()
        {
            DownloadingUrl = "";
            CurrentUrlIndex = 0;
            TotalImageUrlNumber = 0;
            CurrentImageUrlIndex = 0;
        }

        public void SetTotalUrlNumber(int totalUrlNumber)
        {
            TotalUrlNumber = totalUrlNumber;
            _reset();
        }

        public void SetCurrentUrl(Uri uri)
        {
            DownloadingUrl = uri.ToString();
        }

        public void ForwardCurrentUrlIndex()
        {
            CurrentUrlIndex++;
            CurrentImageUrlIndex = 0;
        }

        public void SetTotalImageUrlNumber(int totalImageUrlNumber)
        {
            TotalImageUrlNumber = totalImageUrlNumber;
        }

        public void ForwardCurrentImageUrlIndex()
        {
            CurrentImageUrlIndex++;
        }
    }
}
