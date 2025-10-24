using ReactiveUI;

namespace MangaDownloader.Models
{
    public class Progress : ReactiveObject
    {
        private int _maximumValue = 0;

        private int _currentValue = 0;

        public int Maximum
        {
            get => _maximumValue;
            private set
            {
                if (value >= 0)
                {
                    this.RaiseAndSetIfChanged(ref _maximumValue, value);
                }
            }
        }

        public int Current
        {
            get => _currentValue;
            private set
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
            Maximum = maxValue;
        }

        public void UpdateProgress(int addingValue)
        {
            Current += addingValue;
        }

        public void ResetProgress()
        {
            Maximum = 0;
            Current = 0;
        }
    }
}
