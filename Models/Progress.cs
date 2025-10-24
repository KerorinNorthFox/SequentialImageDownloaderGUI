using ReactiveUI;

namespace MangaDownloader.Models
{
    public class Progress : ReactiveObject
    {
        private int _maxValue = 0;

        private int _currentValue = 0;

        public int Max
        {
            get => _maxValue;
            set
            {
                if (value >= 0)
                {
                    this.RaiseAndSetIfChanged(ref _maxValue, value);
                }
            }
        }

        public int Current
        {
            get => _currentValue;
            set
            {
                if (value >= 0)
                {
                    this.RaiseAndSetIfChanged(ref _currentValue, value);
                }
            }
        }

        public void InitializeProgress(int maxValue)
        {
            ResetProgress();
            Max = maxValue;
        }

        public void UpdateProgress(int addingValue)
        {
            Current += addingValue;
        }

        public void ResetProgress()
        {
            Max = 0;
            Current = 0;
        }
    }
}
