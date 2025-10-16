using System;

namespace MangaDownloader.Models.EventSources
{
    public class InputUrlEventSource
    {
        private event Action<Uri>? addUriEvent;

        public InputUrlEventSource Subscribe(Action<Uri> addUriHandler)
        {
            addUriEvent += addUriHandler;
            return this;
        }

        public void OnAddUri(Uri uri)
        {
            addUriEvent?.Invoke(uri);
        }
    }
}
