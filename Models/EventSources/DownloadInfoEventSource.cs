using System;

namespace MangaDownloader.Models.EventSources
{
    public class DownloadInfoEventSource
    {
        private event Action<int>? _setTotalUrlNumberEvent;

        private event Action<Uri>? _setCurrentUrlEvent;

        private event Action? _forwardCurrentIndexEvent;

        public DownloadInfoEventSource Subscribe(Action<int> setTotalHandler, Action<Uri> currentUriHandler, Action forwardHandler)
        {
            _setTotalUrlNumberEvent += setTotalHandler;
            _setCurrentUrlEvent += currentUriHandler;
            _forwardCurrentIndexEvent += forwardHandler;
            return this;
        }

        public void OnSetTotalUrlNumber(int total)
        {
            _setTotalUrlNumberEvent?.Invoke(total);
        }
        public void OnSetCurrentUrl(Uri uri)
        {
            _setCurrentUrlEvent?.Invoke(uri);
        }

        public void OnForwardCurrentIndex()
        {
            _forwardCurrentIndexEvent?.Invoke();
        }

    }
}
