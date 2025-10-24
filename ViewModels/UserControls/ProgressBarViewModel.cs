using MangaDownloader.Models;

namespace MangaDownloader.ViewModels.UserControls
{
    public class ProgressBarViewModel : ViewModelBase
    {
        public Progress MangaDownloadProgress { get; } = new Progress();

        public Progress ImageDownloadProgress { get; } = new Progress();

        public ProgressBarViewModel()
        {

        }

    }
}
