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

        private DownloadProgressEvents _progress;

        public MainWindowViewModel()
        {
            _config = new Config();
            _progress = new DownloadProgressEvents()
                .Subscribe(
                    ProgressBarViewModel.InitializeProgress,
                    ProgressBarViewModel.UpdateProgress,
                    ProgressBarViewModel.ResetProgress
                    );

            TaskManageViewModel = new TaskManageViewModel(
                    new MangaDownloadService(new ImageDownloader(_config.SelectorJsonPath), _progress)
                );
        }

    }
}
