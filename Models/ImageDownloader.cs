﻿using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MangaDownloader.Models
{
    public class ImageDownloader
    {
        private HttpClient _httpClient = new HttpClient();

        private IBrowsingContext _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        private Selectors _selectors;

        public ImageDownloader()
        {
#if DEBUG
            // Assetsファイルのパスを参照する
            var baseDir = AppDomain.CurrentDomain.BaseDirectory; // \MangaDownloader\bin\Debug\new8.0
            var projectRootDir = Directory.GetParent(baseDir)?.Parent?.Parent?.Parent?.FullName; // \MangaDownloder\
            var jsonPath = Path.Combine(projectRootDir!, "Assets", "selector.json"); // \MangaDownloader\Assets\selector.json
#else
            // TODO :本番環境ではAppDataを参照する
            //       AppDataにフォルダを作るコードを書く必要あり？
            var jsonPath = "";
#endif
            _selectors = new Selectors(jsonPath);
        }

        /// <summary>
        /// ページの連番画像URIのリストを取得する
        /// </summary>
        /// <param name="pageUri">連番画像を取得するページのURI</param>
        /// <returns></returns>
        public async Task<List<Uri>> GetSequentialImagesUris(Uri pageUri)
        {
            var document = await getDocument(pageUri);

            List<Uri> imageUris = new List<Uri>();
            foreach (var imageUri in parseSequentialImagesUri(document, pageUri))
            {
                imageUris.Add(imageUri);
            }

            return imageUris;
        }

        private async Task<IDocument> getDocument(Uri uri)
        {
            IDocument document;
            try
            {
                var resHtml = await _httpClient.GetStringAsync(uri);
                document = await _context.OpenAsync(req => req.Content(resHtml));
            } catch (Exception e)
            {
                throw new FailedDownloadException(e.Message);
            }
            return document;
        }

        private IEnumerable<Uri> parseSequentialImagesUri(IDocument targetDoc, Uri targetUri)
        {
            int index = 0;
            int selectorIndex = 0;
            while(true)
            {
                SelectorMember selector = _selectors[targetUri.Host];

                if (selector.Selectors.Count <= selectorIndex)
                {
                    break;
                }

                string targetSelector = selector.Selectors[selectorIndex];
                targetSelector = targetSelector.Replace("xxxx", (selector.StartIndex+index).ToString());
                Debug.WriteLine($">>selector :{targetSelector}");

                IHtmlImageElement? imageElement = targetDoc.QuerySelector(targetSelector) as IHtmlImageElement;
                if (imageElement == null || imageElement.Source == null)
                {
                    selectorIndex++;
                    continue;
                }

                yield return new Uri(imageElement.Source);
                index++;
            }
        }
        /// <summary>
        /// 画像をダウンロードする
        /// </summary>
        /// <param name="imageUris">ダウンロードする画像のURIのリスト</param>
        /// <returns></returns>
        public async IAsyncEnumerable<Bitmap> DownloadImages(List<Uri> imageUris)
        {
            foreach (var imageUri in imageUris)
            {
                yield return await downloadImage(imageUri);
            }
        }

        private async Task<Bitmap> downloadImage(Uri imageUri) 
        {
            var res = await _httpClient.GetAsync(imageUri);
            if (!res.IsSuccessStatusCode)
            {
                throw new FailedDownloadException($">>Can not get response: {res.StatusCode}");
            }
            using var stream = await res.Content.ReadAsStreamAsync();
            return new Bitmap(stream);
        }

        public static string GenerateSaveDirPathFromUri(string baseDir, Uri uri)
        {
            string saveDirPath = Path.Combine(baseDir, "save");

            foreach (var seg in uri.Segments.Skip(1)) // 最初の"\"をスキップ
            {
                saveDirPath = Path.Combine(saveDirPath, seg);
            }
            if (uri.Query != "")
            {
                saveDirPath = Path.Combine(saveDirPath, uri.Query);
            }

            string[] banWords = ["?", "*", "<", ">", "|", "\""];
            foreach (var word in banWords)
            {
                if (saveDirPath.Contains(word))
                {
                    saveDirPath = saveDirPath.Replace(word, "");
                }
            }

            return saveDirPath;
        }

        public static void CreateSaveDir(string saveDirPath)
        {
            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(saveDirPath);
            }
        }
    }

    public class FailedDownloadException : Exception
    {
        public FailedDownloadException(string message) : base(message) { }
    }
}