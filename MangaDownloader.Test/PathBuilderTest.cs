
using MangaDownloader.Desktop;
using MangaDownloader.Rule;
using System.Diagnostics;
using Xunit;

namespace MangaDownloader.Test
{
    public class PathBuilderTest
    {
        private static readonly Config _config = new Config();

        private string _baseDir = _config.SaveBasePath;

        private PathBuilder _builder;
        public PathBuilderTest()
        {
            _builder = new PathBuilder()
                .Append(_baseDir)
                .Append("save")
                .Append("hoge")
                .Append("fuga");
        }

        [Fact]
        public void PathBuilderTest_1()
        {
            var a = _builder.Build();

            Assert.Equal(a, Path.Combine(_baseDir, "save", "hoge", "fuga"));

        }

        [Fact]
        public void PathBuilderTest_2()
        {
            var b = _builder.Build([1]);

            Assert.Equal(b, Path.Combine(_baseDir, "hoge", "fuga"));
        }

        [Fact]
        public void PathBuilderTest_3()
        {
            var c = _builder.Build([1, 3]);

            Assert.Equal(c, Path.Combine(_baseDir, "hoge"));
        }

        [Fact]
        public void PathBuilderTest_4()
        {
            var d = _builder.Build([2, 5]);

            Assert.Equal(d, Path.Combine(_baseDir, "save", "fuga"));
        }

    }

}
