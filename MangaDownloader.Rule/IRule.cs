using AngleSharp.Dom;

namespace MangaDownloader.Rule
{
    public interface IRule
    {
        /// <summary>
        /// リクエストされたページのhtmlを取得
        /// </summary>
        /// <param name="pageUri"></param>
        /// <returns></returns>
        Task<IDocument> GetDocument(Uri pageUri);

        /// <summary>
        /// 取得したhtmlから
        /// </summary>
        /// <param name="targetDoc"></param>
        /// <param name="tagetUri"></param>
        /// <returns></returns>
        IEnumerable<Uri> ParsePageUri(IDocument targetDoc, string? pageId);

        Task<string> GetTitle();

        Task<string> GetAuthor();
    }
}
