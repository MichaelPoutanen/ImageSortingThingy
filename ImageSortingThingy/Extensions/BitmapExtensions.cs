using System;
using Avalonia;
using Avalonia.Media.Imaging;

namespace ImageSortingThingy.Extensions;

public static class BitmapExtensions
{
    public static Bitmap CreateResizedBitmap(this Bitmap source, int maxWidth, int maxHeight)
    {
        int originalWidth = source.PixelSize.Width;
        int originalHeight = source.PixelSize.Height;

        double widthRatio = (double)maxWidth / originalWidth;
        double heightRatio = (double)maxHeight / originalHeight;
        double scaleFactor = Math.Min(widthRatio, heightRatio);

        int newWidth = (int)(originalWidth * scaleFactor);
        int newHeight = (int)(originalHeight * scaleFactor);

        return source.CreateScaledBitmap(new PixelSize(newWidth, newHeight));
    }
}