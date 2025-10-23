using Avalonia.Data.Converters;
using MangaDownloader.Models;
using System;
using System.Globalization;

namespace MangaDownloader.Converter
{
    public class DownloadStatusToIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DownloadStatus status)
            {
                return status switch
                {
                    DownloadStatus.Pending => "Clock",
                    DownloadStatus.Downloading => "Download",
                    DownloadStatus.Finished => "Check",
                    DownloadStatus.Failed => "WindowClose",
                    _ => "Help"
                };
            }
            return "Help";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}