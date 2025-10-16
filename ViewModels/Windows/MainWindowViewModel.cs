using System;

namespace MangaDownloader.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public TaskManageViewModel TaskManageViewModel { get; }

        public MainWindowViewModel()
        {
            TaskManageViewModel = new TaskManageViewModel();
        }

    }
}
