using MangaDownloader.Models;
using MangaDownloader.Models.Config;
using MangaDownloader.Models.Events;
using MangaDownloader.Services;
using MangaDownloader.ViewModels.UserControls;
using System;

namespace MangaDownloader.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public TaskManageViewModel TaskManageViewModel { get; }

        public ProgressBarViewModel ProgressBarViewModel { get; } = new ProgressBarViewModel();

        private Config _config;

        private DownloadProgressEvents _mangaDownloadProgressEvents;

        private DownloadProgressEvents _imageDownloadProgressEvents;

        public MainWindowViewModel()
        {
            _config = new Config();
            _mangaDownloadProgressEvents = new DownloadProgressEvents()
                .Subscribe(
                    ProgressBarViewModel.MangaDownloadProgress.InitializeProgress,
                    ProgressBarViewModel.MangaDownloadProgress.UpdateProgress,
                    ProgressBarViewModel.MangaDownloadProgress.ResetProgress
                    );
            _imageDownloadProgressEvents = new DownloadProgressEvents()
                .Subscribe(
                    ProgressBarViewModel.ImageDownloadProgress.InitializeProgress,
                    ProgressBarViewModel.ImageDownloadProgress.UpdateProgress,
                    ProgressBarViewModel.ImageDownloadProgress.ResetProgress
                    );

            TaskManageViewModel = new TaskManageViewModel(
                    new MangaDownloadService(new ImageDownloader(_config.SelectorJsonPath), _mangaDownloadProgressEvents, _imageDownloadProgressEvents)
                );
        }

    }
}
