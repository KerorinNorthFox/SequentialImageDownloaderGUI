using AngleSharp.Dom;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MangaDownloader.Models
{
    public interface IImageDownloader
    {
        Task<IDocument> GetDocument(Uri pagUri);

        IEnumerable<Uri> ParsePageUri(IDocument targetDoc, Uri targetUri);

        Task<Bitmap> DownloadImage(Uri imageUri);

        Task<string> GetTitle(IDocument targetDoc, SelectorMember selector);

    }
}
