using Avalonia.Data.Converters;
using Avalonia.Media;
using MangaDownloader.Desktop.Models;
using System;
using System.Globalization;

namespace MangaDownloader.Desktop.Converter
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

    public class DownloadStatusToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DownloadStatus status)
            {
                return status switch
                {
                    DownloadStatus.Pending => Brushes.Orange,
                    DownloadStatus.Downloading => Brushes.Blue,
                    DownloadStatus.Finished => Brushes.Green,
                    DownloadStatus.Failed => Brushes.Red,
                    _ => Brushes.Transparent
                };
            }
            return Brushes.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
