using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;

namespace MangaDownloader.Rule
{
    public class BasicRule : IDisposable, IRule
    {
        public ISelectorMember Selector { get; }

        private IBrowsingContext _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        public BasicRule(ISelectorMember selector)
        {
            Selector = selector;
        }

        public virtual async Task<IDocument> GetDocument(Uri pageUri)
        {
            try
            {
                return await _context.OpenAsync(pageUri.ToString());
            }
            catch (Exception e)
            {
                throw new Exception($"Opening '${pageUri}' is failed!. >> {e.Message}");
            }
        }

        public virtual IEnumerable<Uri> ParsePageUri(IDocument targetDoc, string? pageId)
        {
            int index = 0;
            int selectorIndex = 0;
            while (true)
            {
                if (Selector.ImageSelectors.Count <= selectorIndex) // 候補セレクターを全て回したらbreak
                {
                    break;
                }

                string candidateSelector = Selector.ImageSelectors[selectorIndex];
                candidateSelector = replaceSelector(candidateSelector, index, pageId);

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
        /// 取得したセレクターをindexで置き換えて回す
        /// </summary>
        /// <param name="candidateSelector">取得したセレクター</param>
        /// <param name="index"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        protected virtual string replaceSelector(string candidateSelector, int index, string? pageId)
        {
            return candidateSelector.Replace("xxxx", (Selector.StartNthChildIndex + index).ToString());
        }

        public virtual string? GetTitle(IDocument targetDoc)
        {
            return getTextContent(targetDoc, Selector.TitleSelector);
        }

        public virtual string? GetAuthor(IDocument targetDoc)
        {
            return getTextContent(targetDoc, Selector.AuthorSelector);
        }

        protected virtual string? getTextContent(IDocument targetDoc, string selector)
        {
            IHtmlElement? elem;
            try
            {
                elem = targetDoc.QuerySelector(selector) as IHtmlElement;
            }
            catch
            {
                return null;
            }
            if (elem == null || elem.TextContent == null)
            {
                return null;
            }
            return elem.TextContent;
        }

        public string BuildSaveDirPath(string baseDir, Uri uri, string? title, string? author)
        {
            var builder = new PathBuilder()
                .Append(baseDir)
                .Append(uri.Host)
                .Append(uri.Segments[1..^1].Select((value) => value.Trim('/')).ToArray());

            if (author != null)
            {
                builder = builder.Append(author);
            }

            if (title != null)
            {
                builder = builder.Append($"{title}_{uri.Segments[^1]}");
            }
            else
            {
                builder = builder.Append(uri.Segments[^1]);
            }

            return buildPath(builder);
        }

        protected virtual string buildPath(PathBuilder builder)
        {
            return builder.Build();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
