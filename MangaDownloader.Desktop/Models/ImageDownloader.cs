using Avalonia.Media.Imaging;
using MangaDownloader.Rule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MangaDownloader.Desktop.Models
{
    public class ImageDownloader : IDisposable, IImageDownloader
    {
        private HttpClient _httpClient = new HttpClient();

        private IRuleProvider _rules;

        public ImageDownloader(IRuleProvider rule)
        {
            _rules = rule;
        }

        public IRule? MatchRule(string host)
        {
            if (_rules.TryGetValue(host, out var rule))
            {
                return rule;
            }
            else
            {
                return null;
            }
        }

        public async Task<Bitmap> DownloadImage(Uri imageUri)
        {
            var res = await _httpClient.GetAsync(imageUri);
            if (!res.IsSuccessStatusCode)
            {
                throw new FailedDownloadException($">>Can not get response: {res.StatusCode}");
            }

            using var stream = await res.Content.ReadAsStreamAsync();
            return new Bitmap(stream);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

    public class FailedDownloadException : Exception
    {
        public FailedDownloadException(string message) : base(message) { }
    }

    public enum DownloadStatus
    {
        Pending,
        Downloading,
        Finished,
        Failed
    }
}
