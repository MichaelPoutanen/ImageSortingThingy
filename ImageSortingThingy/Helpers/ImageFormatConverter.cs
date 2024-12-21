using System;
using System.Diagnostics;
using System.IO;
using ImageMagick;

namespace ImageSortingThingy.Helpers;

public static class ImageFormatConverter
{
    public static async void ConvertHeicToJpg(string heicFilePath, string jpgFilePath)
    {
        try
        {
            FileInfo info = new(heicFilePath);
            using (MagickImage image = new(info.FullName))
            {
                await image.WriteAsync(jpgFilePath);
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            Debugger.Break();
#endif
            await MessageBoxHelper.ErrorExceptionMessageBox("Exception in ImageFormatConverter"
                    , $"An exception has occured while trying to convert a heic file to jpg.\nError:\n\n{ex.Message}")
                .ShowAsync();
        }
    }
}