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

#if DEBUG
            // Assetsファイルのパスを参照する
            var baseDir = AppDomain.CurrentDomain.BaseDirectory; // \MangaDownloader\bin\Debug\new8.0
            var projectRootDir = Directory.GetParent(baseDir)?.Parent?.Parent?.Parent?.FullName; // \MangaDownloder\
            var jsonPath = Path.Combine(projectRootDir!, "Assets", "selector.json"); // \MangaDownloader\Assets\selector.json
#else
            // TODO :本番環境ではAppDataを参照する
            //       AppDataにフォルダを作るコードを書く必要あり？
            var jsonPath = selectorJsonPath;
#endif

            var service = new MangaDownloadService(
                new ImageDownloader(new Selectors(jsonPath)),
                _mangaDownloadProgressEvents,
                _imageDownloadProgressEvents
                );
            TaskManageViewModel = new TaskManageViewModel(service);
        }

    }
}
