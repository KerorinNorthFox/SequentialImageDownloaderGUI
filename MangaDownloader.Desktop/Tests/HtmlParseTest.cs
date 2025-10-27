using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MangaDownloader.Tests
{
    public class HtmlParseTest
    {
        [Fact]
        public void Test()
        {
            task().ContinueWith(x =>
            {
                Console.WriteLine("done");
            });
        }

        private async Task task()
        {
            HttpClient httpClient = new HttpClient();
            var res = await httpClient.GetStringAsync(new Uri("https://momon-ga.com/fanzine/mo2789627/"));
            var parser = new HtmlParser();
            var doc = await parser.ParseDocumentAsync(res);
            Console.WriteLine(doc);

        }
    }
}
