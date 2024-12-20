using System;

namespace ImageSortingThingy.Helpers;

public static class GlobalDefinitions
{
    // TODO: This has to be configurable and preferable in AppData or something similar, but for debugging and developing this will do right now
    public static readonly string ThumbnailStoragePath =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}/exclude_imageSortingThingy/";

    public static readonly int MaxThumbnailSizeX = 512;
    public static readonly int MaxThumbnailSizeY = 512;

    public enum GeneralErrorCodes
    {
        NoError,
        ImageAlreadyExists,
        ImageNotFound,
        ImageFileDefective
    }
}