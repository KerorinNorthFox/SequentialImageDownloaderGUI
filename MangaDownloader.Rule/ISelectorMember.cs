
namespace MangaDownloader.Rule
{
    public interface ISelectorMember
    {
        string Domain { get; }

        List<string> ImageSelectors { get; }

        int StartNthChildIndex { get; }

        string TitleSelector { get; }

        string AuthorSelector { get; }
    }
}