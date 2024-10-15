using System;
using System.Configuration;

namespace MangaDownloader.Models.Config
{
    public class Config
    {


        public Config()
        {
            var a = ConfigurationManager.AppSettings;
        }
    }
}
