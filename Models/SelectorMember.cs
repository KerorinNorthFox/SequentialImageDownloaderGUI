using Newtonsoft.Json;
using System.Collections.Generic;

namespace MangaDownloader.Models
{
    [JsonObject("Selector")]
    public class SelectorMember
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
