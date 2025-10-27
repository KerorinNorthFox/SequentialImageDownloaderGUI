using Avalonia.Data;
using Avalonia.Data.Converters;
using MangaDownloader.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MangaDownloader.Desktop.Converter
{
    // MangaTaskのタイトルが設定されてない場合、Uriを返すコンバーター
    public class MangaTitleOrUriConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values == null || values.Count != 2)
            {
                return BindingOperations.DoNothing;
            }

            string? title = null;
            Uri? uri = null;

            foreach (var item in values)
            {
                switch (item)
                {
                    case string s:
                        title = s;
                        break;
                    case Uri u:
                        uri = u;
                        break;
                }
            }

            if (title == null || uri == null)
            {
                return BindingOperations.DoNothing;
            }

            return title != string.Empty ? title : uri.ToString();
        }
    }
}
