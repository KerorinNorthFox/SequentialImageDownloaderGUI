using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MangaDownloader.Converter
{
    public class IndexConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count != 2)
            {
                return BindingOperations.DoNothing;
            }
            return $"{values[0]}/{values[1]}";
        }
    }
}
