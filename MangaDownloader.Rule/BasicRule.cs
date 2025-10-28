using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MangaDownloader.Rule
{
    public class BasicRule : IRule
    {
        private IMember _member;

        private IBrowsingContext _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

        public BasicRule(IMember member)
        {
            _member = member;
        }

        public async Task<IDocument> GetDocument(Uri pageUri)
        {
            try
            {
                return await _context.OpenAsync(pageUri.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Uri> ParsePageUri(IDocument targetDoc, string? pageId)
        {
            int index = 0;
            int selectorIndex = 0;
            while (true)
            {
                if (_member.Selectors.Count <= selectorIndex) // 候補セレクターを全て回したらbreak
                {
                    break;
                }

                string candidateSelector = _member.Selectors[selectorIndex];
                candidateSelector = adjustSelector(candidateSelector, index, pageId);

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
        private string adjustSelector(string candidateSelector, int index, string? pageId)
        {
            candidateSelector = candidateSelector.Replace("xxxx", (_member.StartNthChildIndex + index).ToString());
            if (_member.IsNecessaryFileNumber && pageId != null)
            {
                candidateSelector = candidateSelector.Replace("yyyy", Regex.Replace(pageId, @"\D", "")); // pageIdの取得：targetUri.Segments[^1]
            }
            else if (_member.IsNecessaryFileNumber && pageId == null)
            {
                throw new PageIdRequiredException(">>pageId is null.");
            }
            Debug.WriteLine($">>Adjustted Selector :{candidateSelector}");

            return candidateSelector;
        }

        public async Task<string> GetTitle()
        {
            return "";
        }

        public async Task<string> GetAuthor()
        {
            return "";
        }
    }

    public class PageIdRequiredException : Exception
    {
        public PageIdRequiredException(string message) : base(message) { }
    }
}
