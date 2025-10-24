using ReactiveUI;

namespace MangaDownloader.Models
{
    public class Progress : ReactiveObject
    {
        private int _value = 0;

        private int _maxValue = 0;

        private int _currentValue = 0;

        public int Value
        {
            get => _value;
            set
            {
                if (value >= 0)
                {
                    this.RaiseAndSetIfChanged(ref _value, value);
                }
            }
        }

        public int Max
        {
            get => _maxValue;
            set => this.RaiseAndSetIfChanged(ref _maxValue, value);
        }

        public int Current
        {
            get => _currentValue;
            set => this.RaiseAndSetIfChanged(ref _currentValue, value);
        }

        public void InitializeProgress(int maxValue)
        {
            ResetProgress();
            Value += maxValue;
            Max = maxValue;
        }

        public void UpdateProgress(int addingValue)
        {
            Value += addingValue;
            Current += addingValue;
        }

        public void ResetProgress()
        {
            Value = 0;
            Max = 0;
            Current = 0;
        }
    }
}
