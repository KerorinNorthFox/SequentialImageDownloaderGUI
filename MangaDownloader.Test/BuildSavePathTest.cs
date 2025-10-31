
using MangaDownloader.Desktop;
using MangaDownloader.Rule;
using Xunit;

namespace MangaDownloader.Test
{
    public class BuildSavePathTest
    {

        private IRule rule = new BasicRule(new SelectorMember
        {
            Domain = "example.com",
            ImageSelectors = new List<string>
                {
                    "a"
                },
            StartNthChildIndex = 1,
            TitleSelector = "",
            AuthorSelector = ""
        });

        private static readonly Config _config = new Config();

        private string _baseDir = _config.SaveBasePath;

        private Uri _uri = new Uri("https://example.com/fan/000111");

        [Fact]
        public void BuildSavePathTest_1()
        {
            var x = rule.BuildSaveDirPath(_baseDir, _uri, null, null);

            var y = new PathBuilder()
                .Append(_baseDir)
                .Append("example.com")
                .Append("fan")
                .Append("000111")
                .Build();

            Assert.Equal(x, y);
        }

        [Fact]
        public void BuildSavePathTest_2()
        {
            var x = rule.BuildSaveDirPath(_baseDir, _uri, "kemomimi", null);

            var y = new PathBuilder()
                .Append(_baseDir)
                .Append("example.com")
                .Append("fan")
                .Append("kemomimi_000111")
                .Build();

            Assert.Equal(x, y);
        }

        [Fact]
        public void BuildSavePathTest_3()
        {
            var x = rule.BuildSaveDirPath(_baseDir, _uri, null, "kerorinnf");

            var y = new PathBuilder()
                .Append(_baseDir)
                .Append("example.com")
                .Append("fan")
                .Append("kerorinnf")
                .Append("000111")
                .Build();

            Assert.Equal(x, y);
        }

        [Fact]
        public void BuilSavePathTest_4()
        {
            var x = rule.BuildSaveDirPath(_baseDir, _uri, "kemomimi", "kerorinnf");

            var y = new PathBuilder()
                .Append(_baseDir)
                .Append("example.com")
                .Append("fan")
                .Append("kerorinnf")
                .Append("kemomimi_000111")
                .Build();

            Assert.Equal(x, y);
        }
    }
}
