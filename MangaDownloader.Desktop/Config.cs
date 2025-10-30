using System;
using System.Configuration;
using System.IO;

namespace MangaDownloader.Desktop
{
    public class Config
    {
        private string _baseDir = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 画像保存先ディレクトリのパス
        /// </summary>
        public string SaveBasePath { get; set; } = string.Empty;

        public string PluginPath { get; set; } = string.Empty;

        public Config()
        {
#if DEBUG
            // Assetsファイルのパスを参照する
            var baseDir = _baseDir; // \MangaDownloader\bin\Debug\new8.0
            var projectRootDir = Directory.GetParent(baseDir)?.Parent?.Parent?.Parent?.FullName; // \MangaDownloder\
            var assetsDir = Path.Combine(projectRootDir!, "Assets"); // \MangaDownloader\Assets\
#else
            // TODO :本番環境ではApp.configを参照する
#endif
            SaveBasePath = Path.Combine(baseDir, "Save");
            if (!Directory.Exists(SaveBasePath))
            {
                Directory.CreateDirectory(SaveBasePath);
            }

            PluginPath = Path.Combine(assetsDir, "Plugins");

            var a = ConfigurationManager.AppSettings;
        }
    }
}
