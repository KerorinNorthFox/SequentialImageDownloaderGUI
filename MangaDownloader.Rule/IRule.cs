using AngleSharp.Dom;

namespace MangaDownloader.Rule
{
    public interface IRule
    {
        ISelectorMember Selector { get; }

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

        string? GetTitle(IDocument targetDoc);

        string? GetAuthor(IDocument targetDoc);

        string BuildSaveDirPath(string baseDir, Uri uri, string? title, string? author);
    }
}
