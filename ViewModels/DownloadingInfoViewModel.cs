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
                DownloadingUrl = "https://example.com";
            }
        }

        private int _totalUrlNumber;

        public int TotalUrlNumber
        {
            get => _totalUrlNumber;
            set => this.RaiseAndSetIfChanged(ref _totalUrlNumber, value);
        }

        private int _currentUrlIndex;

        public int CurrentUrlIndex
        {
            get => _currentUrlIndex;
            set => this.RaiseAndSetIfChanged(ref _currentUrlIndex, value);
        }

        private string _downloadingUrl = string.Empty;

        public string DownloadingUrl
        {
            get => _downloadingUrl;
            set => this.RaiseAndSetIfChanged(ref _downloadingUrl, value);
        }

        public void SetTotalUrlNumber(int totalUrlNumber)
        {
            _totalUrlNumber = totalUrlNumber;
        }

        public void SetCurrentUrl(Uri uri)
        {
            DownloadingUrl = uri.ToString();
        }

        public void ForwardCurrentUrlIndex()
        {
            CurrentUrlIndex++;
        }
    }
}
