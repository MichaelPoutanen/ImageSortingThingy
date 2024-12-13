using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ImageSortingThingy.Models;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Directory = MetadataExtractor.Directory;

namespace ImageSortingThingy.Extensions;

public static class ImageFileListEntryModelCastingExtension
{
    public static ImageFileListEntryModel ToImageFileListEntryModel(this string filePath, int fileId)
    {
        IReadOnlyList<Directory> metaDataDirectories=ImageMetadataReader.ReadMetadata(filePath);
        ExifSubIfdDirectory? pictureTakenExifDirectory=metaDataDirectories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

        
        // This chonky boy sets the imageCreatedDateTime to the EXIF date specified in the image file, if it can parse it.
        // If the parsing works, it creates a temporary DateTime variable called tmp and returns it, otherwise it gets set
        // to default(DateTime).
        // The chosen format here is what my stuff uses per default. In the future it might make sense to implement a list of common
        // formats which it iterates through, maybe?
        DateTime imageCreatedDateTime = DateTime.TryParse(
            pictureTakenExifDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal), out DateTime tmp)
            ? tmp
            : DateTime.TryParseExact(pictureTakenExifDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal),
                "yyyy:MM:dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime tmp2)
                ? tmp2
                : default;
        

        return new ImageFileListEntryModel
        {
            FileId = fileId,
            AbsolutePath = filePath,
            FileName = Path.GetFileName(filePath),
            CustomName = string.Empty,
            Description = string.Empty,
            FileCreationDateTime = File.GetCreationTimeUtc(filePath),
            ImageCreatedDateTime = imageCreatedDateTime
        };
    }
}