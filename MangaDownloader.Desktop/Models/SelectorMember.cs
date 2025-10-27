using Newtonsoft.Json;
using System.Collections.Generic;

namespace MangaDownloader.Models
{
    [JsonObject("selector")]
    public class SelectorMember
    {
        /// <summary>
        /// ホスト名
        /// </summary>
        [JsonProperty("host")]
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// 連番画像の存在する箇所のセレクター
        /// </summary>
        [JsonProperty("selectors")]
        public List<string> Selectors { get; set; } = new List<string>();

        /// <summary>
        /// 連番画像のnth-childの始点
        /// </summary>
        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        /// <summary>
        /// 連番画像のセレクターにページ固有のidが必要かどうか
        /// </summary>
        [JsonProperty("isNecessaryFileNumber")]
        public bool IsNecessaryFileNumber { get; set; }

    }
}
