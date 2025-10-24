using MangaDownloader.Models;
using ReactiveUI;

namespace MangaDownloader.ViewModels.UserControls
{
    public class ProgressBarViewModel : ViewModelBase
    {
        public Progress MangaDownloadProgress = new Progress();

        public Progress ImageDownloadProgress = new Progress();

        public ProgressBarViewModel()
        {

        }

    }
}
