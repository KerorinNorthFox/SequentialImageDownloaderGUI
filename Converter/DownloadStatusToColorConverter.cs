using Avalonia.Data.Converters;
using Avalonia.Media;
using MangaDownloader.Models;
using System;
using System.Globalization;

namespace MangaDownloader.Converter
{
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