using System.Configuration;

namespace MangaDownloader.Models.Config
{
    public class Config
    {
        /// <summary>
        /// セレクターを定義したjsonファイルのパス
        /// </summary>
        public string SelectorJsonPath { get; private set; } = string.Empty;

        /// <summary>
        /// 画像保存先ディレクトリのパス
        /// </summary>
        public string SaveBasePath { get; private set; } = string.Empty;

        public Config()
        {
            SelectorJsonPath = "C:\\Users\\masat\\Desktop\\program\\csharp_projects\\MangaDownloader\\Assets\\selector.json";
            var a = ConfigurationManager.AppSettings;
        }

        public void UpdateSelectorJsonPath(string selectorJsonPath)
        {
            SelectorJsonPath = selectorJsonPath;
        }

        public void UpdateSaveBasePath(string saveBasePath)
        {
            SaveBasePath = saveBasePath;
        }
    }
}
