using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloader.Models
{
    [JsonObject("Selector")]
    internal class SelectorMember
    {
        [JsonProperty("Host")]
        public string Host { get; set; } = string.Empty;

        [JsonProperty("Selectors")]
        public List<string> Selectors { get; set; } = new List<string>();

        [JsonProperty("StartIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("IsNecessaryFileNumber")]
        public bool IsNecessaryFileNumber { get; set; }

    }
}
