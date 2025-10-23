using Avalonia.Media.Imaging;
using System;

namespace MangaDownloader.Models
{
    public class Page
    {
        public int Index { get; }
        public Uri Uri { get; }

        public Bitmap Image { get; }

        public Page(int index, Uri uri, Bitmap image)
        {
            Index = index;
            Uri = uri;
            Image = image;
        }

    }
}
