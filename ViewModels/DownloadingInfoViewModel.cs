using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private string _downloadingUrl = string.Empty;

        public string DownloadingUrl
        {
            get => _downloadingUrl;
            set => this.RaiseAndSetIfChanged(ref _downloadingUrl, value);
        }
    }
}
