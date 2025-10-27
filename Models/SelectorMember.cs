using Newtonsoft.Json;
using System.Collections.Generic;

namespace MangaDownloader.Models
{
    [JsonObject("selector")]
    public class SelectorMember
    {
        [JsonProperty("host")]
        public string Host { get; set; } = string.Empty;

        [JsonProperty("selectors")]
        public List<string> Selectors { get; set; } = new List<string>();

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("isNecessaryFileNumber")]
        public bool IsNecessaryFileNumber { get; set; }

    }
}
