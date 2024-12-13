using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

namespace ImageSortingThingy.Converters;

public class FilePathToBitmapConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string filePath && File.Exists(filePath))
        {
            try
            {
                return new Bitmap(filePath);
            }
            catch (Exception ex)
            {
                //TODO: Log ex
#if DEBUG
                Debugger.Break();
#endif
                return null;
            }
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}