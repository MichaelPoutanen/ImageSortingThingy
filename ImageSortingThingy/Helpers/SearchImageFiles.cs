using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageSortingThingy.Helpers;

public static class SearchImageFiles
{
    /// <summary>
    /// Returns ALL images files from the given directory and all sub-directories (recursive search)
    /// </summary>
    /// <param name="directory">The base directory</param>
    /// <returns>List of all found image files</returns>
    public static string[] GetAllImageFiles(string directory)
    {
        return FilterFilesInFolder(directory, ["jpg", "jpeg", "png", "gif", "tiff"], true);
    }

    public static string[] GetAppleImageFiles(string directory)
    {
        return FilterFilesInFolder(directory, ["heic"], true);

    }

    /// <summary>
    /// Returns image files from the given directory only
    /// </summary>
    /// <param name="directory">Search Directory</param>
    /// <returns>List of all found image files</returns>
    public static string[] GetImageFilesInDirectory(string directory)
    {
        return FilterFilesInFolder(directory, ["jpg", "jpeg", "png", "gif", "tiff"], false);
    }

    public static string[] FilterFilesInFolder(string searchFolder, string[] filters, bool isRecursive)
    {
        List<string> filesFound = new List<string>();
        SearchOption searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        foreach (string filter in filters)
        {
            filesFound.AddRange(Directory.GetFiles(searchFolder, $"*.{filter}", searchOption));
        }

        return filesFound.Where(s => !s.ToLower().Contains("exclude")).ToArray();
    }
}