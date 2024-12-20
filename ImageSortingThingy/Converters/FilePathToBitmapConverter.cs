using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using ImageSortingThingy.Extensions;
using ImageSortingThingy.Helpers;

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
                // Generate a unique thumbnail path based on the original file path
                string thumbnailPath = GetThumbnailPath(filePath);

                if (File.Exists(thumbnailPath))
                {
                    // Load the existing thumbnail
                    return new Bitmap(thumbnailPath);
                }
                else
                {
                    // Generate the thumbnail
                    Bitmap bitmap = new Bitmap(filePath);

                    // Resize to a smaller size (e.g., 100x100 pixels)
                    Bitmap resizedBitmap = bitmap.CreateResizedBitmap(GlobalDefinitions.MaxThumbnailSizeX,
                        GlobalDefinitions.MaxThumbnailSizeY);

                    // Save the resized bitmap to the thumbnail directory
                    SaveBitmapToFile(resizedBitmap, thumbnailPath);

                    // Return the resized bitmap
                    return resizedBitmap;
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                //TODO: Log Ex
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

    private string GetThumbnailPath(string originalPath)
    {
        string fileName = Path.GetFileNameWithoutExtension(originalPath);
        string extension = ".jpg";
        string sanitizedFileName = $"{fileName}_thumb{extension}";

        if (!Directory.Exists(GlobalDefinitions.ThumbnailStoragePath))
        {
            Directory.CreateDirectory(GlobalDefinitions.ThumbnailStoragePath);
        }

        return Path.Combine(GlobalDefinitions.ThumbnailStoragePath, sanitizedFileName);
    }

    private void SaveBitmapToFile(Bitmap bitmap, string filePath)
    {
        using (FileStream stream = File.Create(filePath))
        {
            bitmap.Save(stream);
        }
    }
}