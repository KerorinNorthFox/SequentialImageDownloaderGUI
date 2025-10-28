using System.Text.RegularExpressions;

namespace MangaDownloader.Rule
{
    public class IdRequiredRule : BasicRule
    {
        public IdRequiredRule(ISelectorMember member) : base(member) { }

        protected override string replaceSelector(string candidateSelector, int index, string? pageId)
        {
            if (pageId == null)
            {
                throw new PageIdRequiredException(">>pageId is null");
            }

            var baseCandidateSelector = base.replaceSelector(candidateSelector, index, pageId);
            return baseCandidateSelector.Replace("yyyy", Regex.Replace(pageId, @"\D", "")); // pageIdの取得：targetUri.Segments[^1]
        }
    }
    public class PageIdRequiredException : Exception
    {
        public PageIdRequiredException(string message) : base(message) { }
    }
}
