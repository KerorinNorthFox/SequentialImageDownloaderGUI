using Avalonia.Media.Imaging;
using System;

namespace MangaDownloader.Models
{
    public class Page
    {
        public Uri Uri { get; }

        public Bitmap Image { get; }

        public Page(Uri uri, Bitmap image)
        {
            Uri = uri;
            Image = image;
        }

    }
}
