using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MangaDownloader.Models
{
    // <summary>
    // ドメインを指定することでセレクターを選択できる辞書クラス
    // <\summary>
    internal class Selectors : Dictionary<string, SelectorMember>
    {
        private string _selectorJsonPath;

        public Selectors(string selectorJsonPath)
        {
            _selectorJsonPath = selectorJsonPath;
            load(_selectorJsonPath);
        }

        private void load(string selectorJsonFile)
        {
            this.Clear();

            string jsonContent = File.ReadAllText(selectorJsonFile, Encoding.UTF8);
            List<SelectorMember>? jsonData = JsonConvert.DeserializeObject<List<SelectorMember>>(jsonContent);

            if (jsonData == null)
            {
                throw new Exception($"Incorrect json structure in {selectorJsonFile}");
            }

            foreach (SelectorMember data in jsonData)
            {
                Add(data.Host, data);
            }
        }

        public void Reload()
        {
            load(_selectorJsonPath);
        }
    }
}
