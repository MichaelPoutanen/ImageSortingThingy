using System;
using System.Collections.Generic;
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
        // to default(DateTime)
        DateTime imageCreatedDateTime = DateTime.TryParse(
            pictureTakenExifDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal), out DateTime tmp) 
            ? tmp : default;
        
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