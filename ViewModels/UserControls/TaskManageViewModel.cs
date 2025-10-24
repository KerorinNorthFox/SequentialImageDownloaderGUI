using MangaDownloader.Services;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MangaDownloader.ViewModels
{
    public partial class TaskManageViewModel : ViewModelBase
    {
        public InputUrlViewModel InputUrlViewModel { get; }

        public MangaListViewModel MangaListViewModel { get; } = new MangaListViewModel();

        private IDownloadService _service;

        private bool _isDownloading = false;
        public TaskManageViewModel(IDownloadService service)
        {
            _service = service;
            InputUrlViewModel = new InputUrlViewModel(MangaListViewModel.AddManga);

            var canStartDownload = this.WhenAnyValue(x => x.IsDownloading, x => x.MangaListViewModel.MangaList, (isDownloading, mangaList) => !isDownloading && mangaList.Count > 0);
            StartDownloadCommand = ReactiveCommand.CreateFromTask(startDownload, canStartDownload);

            this.WhenAnyValue(x => x.IsDownloading)
                .Subscribe(isDownloading =>
                {
                    InputUrlViewModel.IsDownloading = isDownloading;
                    MangaListViewModel.IsDownloading = isDownloading;
                });
        }

        public bool IsDownloading
        {
            get => _isDownloading;
            set => this.RaiseAndSetIfChanged(ref _isDownloading, value);
        }

        public ICommand StartDownloadCommand { get; }

        private async Task startDownload()
        {
            if (IsDownloading)
            {
                return;
            }

            IsDownloading = true;

            await _service.DownloadAllAsync(MangaListViewModel.MangaList);

            IsDownloading = false;
        }
    }
}
