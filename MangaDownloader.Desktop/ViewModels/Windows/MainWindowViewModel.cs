using MangaDownloader.Desktop.Models;
using MangaDownloader.Desktop.Models.Events;
using MangaDownloader.Desktop.Services;
using System;
using System.IO;

namespace MangaDownloader.Desktop.ViewModels
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

            var service = new MangaDownloadService(
                new ImageDownloader(_config),
                _mangaDownloadProgressEvents,
                _imageDownloadProgressEvents
                );
            TaskManageViewModel = new TaskManageViewModel(service);
        }

    }
}
