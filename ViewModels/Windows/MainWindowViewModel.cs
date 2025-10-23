using MangaDownloader.Models.Config;
using System;

namespace MangaDownloader.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public TaskManageViewModel TaskManageViewModel { get; }

        private Config _config;

        public MainWindowViewModel()
        {
            _config = new Config();
            TaskManageViewModel = new TaskManageViewModel(_config);
        }

    }
}
