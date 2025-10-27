using Avalonia.Media.Imaging;
using System;

namespace MangaDownloader.Desktop.Models
{
    public class Page : IDisposable
    {
        public int Index { get; }

        public Uri Uri { get; }

        public Bitmap Image { get; }

        private bool _disposed = false;

        public Page(int index, Uri uri, Bitmap image)
        {
            Index = index;
            Uri = uri;
            Image = image;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Image?.Dispose();
                }
                _disposed = true;
            }
        }

        ~Page()
        {
            Dispose(false);
        }
    }
}