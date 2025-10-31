using MangaDownloader.Desktop.Models;
using MangaDownloader.Desktop.Models.Events;
using MangaDownloader.Desktop.Services;

namespace MangaDownloader.Desktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public TaskManageViewModel TaskManageViewModel { get; }

        public ProgressBarViewModel ProgressBarViewModel { get; } = new ProgressBarViewModel();

        private Config _config;

        private DownloadProgressEvents _mangaDownloadProgressEvents;

        public MainWindowViewModel()
        {
            _config = new Config();
            _mangaDownloadProgressEvents = new DownloadProgressEvents()
                .Subscribe(
                    ProgressBarViewModel.MangaDownloadProgress.InitializeProgress,
                    ProgressBarViewModel.MangaDownloadProgress.UpdateProgress,
                    ProgressBarViewModel.MangaDownloadProgress.ResetProgress
                    );

            var service = new MangaDownloadService(
                new Rules(_config.PluginPath),
                _mangaDownloadProgressEvents,
                _config
                );
            TaskManageViewModel = new TaskManageViewModel(service);
        }

    }
}
