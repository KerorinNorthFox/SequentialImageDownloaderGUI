using System;

namespace MangaDownloader.Models.EventSources
{
    public class DownloadInfoEventSource
    {
        private event Action<int>? _setTotalUrlNumberEvent;

        private event Action<Uri>? _setCurrentUrlEvent;

        private event Action? _forwardCurrentUrlIndexEvent;

        private event Action<int>? _setTotalImageUrlNumberEvent;

        private event Action? _forwardCurrentImageUrlIndexEvent;

        public DownloadInfoEventSource Subscribe(Action<int> setTotalHandler, Action<Uri> currentUriHandler, Action forwardHandler, Action<int> setTotalImageHandler, Action forwardImageHandler)
        {
            _setTotalUrlNumberEvent += setTotalHandler;
            _setCurrentUrlEvent += currentUriHandler;
            _forwardCurrentUrlIndexEvent += forwardHandler;
            _setTotalImageUrlNumberEvent += setTotalImageHandler;
            _forwardCurrentImageUrlIndexEvent += forwardImageHandler;
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

        public void OnForwardCurrentUrlIndex()
        {
            _forwardCurrentUrlIndexEvent?.Invoke();
        }

        public void OnSetTotalImageUrlNumber(int total)
        {
            _setTotalImageUrlNumberEvent?.Invoke(total);
        }

        public void OnForwardCurrentImageUrlIndex()
        {
            _forwardCurrentImageUrlIndexEvent?.Invoke();
        }
    }
}
