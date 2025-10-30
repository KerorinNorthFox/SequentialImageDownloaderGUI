using Avalonia.Media.Imaging;
using MangaDownloader.Rule;
using System;
using System.Threading.Tasks;

namespace MangaDownloader.Desktop.Models
{
    public interface IImageDownloader
    {
        IRule? MatchRule(string host);

        Task<Bitmap> DownloadImage(Uri imageUri);
    }
}
