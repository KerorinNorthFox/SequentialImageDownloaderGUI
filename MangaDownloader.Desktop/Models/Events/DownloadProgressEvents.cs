using System;

namespace MangaDownloader.Desktop.Models.Events
{
    public class DownloadProgressEvents : IProgressEvents
    {
        private event Action<int>? _initializeEvent;

        private event Action<int>? _updateEvent;

        private event Action? _resetEvent;

        public DownloadProgressEvents Subscribe(Action<int> initializeHandler, Action<int> updateHandler, Action resetHandler)
        {
            _initializeEvent += initializeHandler;
            _updateEvent += updateHandler;
            _resetEvent += resetHandler;
            return this;
        }

        public void OnInitializeProgress(int maxvalue)
        {
            _initializeEvent?.Invoke(maxvalue);
        }

        public void OnUpdateProgress(int addingValue)
        {
            _updateEvent?.Invoke(addingValue);
        }

        public void OnResetProgress()
        {
            _resetEvent?.Invoke();
        }
    }
}
