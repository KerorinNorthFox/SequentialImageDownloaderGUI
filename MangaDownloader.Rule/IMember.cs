
namespace MangaDownloader.Rule
{
    public interface IMember
    {
        string Domain { get; }

        List<string> Selectors { get; }

        int StartNthChildIndex { get; }

        Boolean IsNecessaryFileNumber { get; }

        string TitleSelector { get; }

        string AuthorSelector { get; }
    }
}