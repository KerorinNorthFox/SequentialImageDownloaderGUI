using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MangaDownloader.Desktop.Models
{
    public class ImageDownloader : IDisposable, IImageDownloader
    {
        private HttpClient _httpClient = new HttpClient();

        private IBrowsingContext _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        private ISelectorProvider _selectors;

        public ImageDownloader(ISelectorProvider selectors)
        {
            _selectors = selectors;
        }

        public async Task<IDocument> GetDocument(Uri uri)
        {
            try
            {
                return await _context.OpenAsync(uri.ToString());

            }
            catch (Exception e)
            {
                throw new FailedDownloadException(e.Message);
            }
        }

        public IEnumerable<Uri> ParsePageUri(IDocument targetDoc, Uri targetUri)
        {
            int index = 0;
            int selectorIndex = 0;
            while (true)
            {
                SelectorMember selector = _selectors[targetUri.Host];

                if (selector.Selectors.Count <= selectorIndex) // 候補セレクターを全て回したらbreak
                {
                    break;
                }

                string candidateSelector = selector.Selectors[selectorIndex];
                candidateSelector = adjustSelector(candidateSelector, selector, index, targetUri);

                IHtmlImageElement? imageElement = targetDoc.QuerySelector(candidateSelector) as IHtmlImageElement;
                if (imageElement == null || imageElement.Source == null) // imgタグではない、又はimgタグのsrcプロパティが存在しない場合、次の候補セレクターに回す
                {
                    selectorIndex++;
                    continue;
                }

                yield return new Uri(imageElement.Source);
                index++;
            }
        }

        /// <summary>
        /// 取得したセレクターをuri情報で調整する
        /// </summary>
        /// <param name="candidateSelector">取得したセレクター</param>
        /// <param name="selector">当該ホストのセレクター情報オブジェクト</param>
        /// <param name="index"></param>
        /// <param name="targetUri"></param>
        /// <returns></returns>
        private static string adjustSelector(string candidateSelector, SelectorMember selector, int index, Uri targetUri)
        {
            candidateSelector = candidateSelector.Replace("xxxx", (selector.StartIndex + index).ToString());
            if (selector.IsNecessaryFileNumber)
            {
                candidateSelector = candidateSelector.Replace("yyyy", Regex.Replace(targetUri.Segments[^1], @"\D", ""));
            }
            Debug.WriteLine($">>Adjustted Selector :{candidateSelector}");

            return candidateSelector;
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

        public async Task<string> GetTitle(IDocument targetDoc, SelectorMember selector)
        {
            return "";
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _context.Dispose();
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
