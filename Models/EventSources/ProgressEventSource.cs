using System;

namespace MangaDownloader.Models.EventSources
{
    public class ProgressEventSource
    {
        public event Action<int>? SetMaxProgressEvent;

        public event Action<int>? UpdateProgressEvent;

        public event Action? ResetProgressEvent;

        public ProgressEventSource Subscribe(Action<int> setMaxHandler, Action<int> updateHandler, Action resetHandler)
        {
            SetMaxProgressEvent += setMaxHandler;
            UpdateProgressEvent += updateHandler;
            ResetProgressEvent += resetHandler;
            return this;
        }

        public void OnSetMaxProgress(int maxValue)
        {
            SetMaxProgressEvent?.Invoke(maxValue);
        }

        public void OnUpdateProgress(int addingValue)
        {
            UpdateProgressEvent?.Invoke(addingValue);
        }

        public void OnResetProgress()
        {
            ResetProgressEvent?.Invoke();
        }
    }
}
