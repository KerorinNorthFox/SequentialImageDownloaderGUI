using ReactiveUI;

namespace MangaDownloader.ViewModels.UserControls
{
    public class ProgressBarViewModel : ViewModelBase
    {
        private int _progress = 0;

        public ProgressBarViewModel()
        {

        }

        public int Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }

        public void InitializeProgress(int maxValue)
        {
            Progress += maxValue;
        }

        public void UpdateProgress(int addingValue)
        {
            Progress += addingValue;
        }

        public void ResetProgress()
        {
            Progress = 0;
        }
    }
}
