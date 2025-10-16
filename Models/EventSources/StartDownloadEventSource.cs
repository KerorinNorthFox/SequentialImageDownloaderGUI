using System;

namespace MangaDownloader.Models.EventSources
{
    public class StartDownloadEventSource
    {
        private event Action? startDownloadEvent;

        public StartDownloadEventSource Subscribe(Action? startDownloadEventHandler)
        {
            startDownloadEvent += startDownloadEventHandler;
            return this;
        }

        public void OnStartDownload()
        {
            startDownloadEvent?.Invoke();
        }
    }
}
