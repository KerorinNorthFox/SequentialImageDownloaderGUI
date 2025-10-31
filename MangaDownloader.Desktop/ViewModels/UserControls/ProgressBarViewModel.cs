using MangaDownloader.Desktop.Models;

namespace MangaDownloader.Desktop.ViewModels
{
    public class ProgressBarViewModel : ViewModelBase
    {
        public Progress MangaDownloadProgress { get; } = new Progress();

        public ProgressBarViewModel()
        {

        }

    }
}
