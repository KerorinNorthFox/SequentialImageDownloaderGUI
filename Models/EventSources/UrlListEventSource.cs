using System;

namespace MangaDownloader.Models.EventSources
{
    public class UrlListEventSource
    {
        public event Action<Uri>? AddUrlToListEvent;

        public UrlListEventSource Subscribe(Action<Uri> addUrlToListHandler)
        {
            AddUrlToListEvent += addUrlToListHandler;
            return this;
        }

        public void OnAddUrlToList(Uri uri)
        {
            AddUrlToListEvent?.Invoke(uri);
        }
    }
}
