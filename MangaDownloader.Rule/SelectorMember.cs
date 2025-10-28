
namespace MangaDownloader.Rule
{
    public record SelectorMember : ISelectorMember
    {
        public required string Domain { get; set; }
        public required List<string> ImageSelectors { get; set; }
        public required int StartNthChildIndex { get; set; }
        public required string TitleSelector { get; set; }
        public required string AuthorSelector { get; set; }
    }
}
