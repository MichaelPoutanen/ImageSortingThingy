using System;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace ImageSortingThingy.Models;

public class ImageFileListEntryModel
{
    // Primary Key, for DB
    public int Id { get; set; }
    public string? FileName { get; set; }
    public string? AbsolutePath { get; set; }
    public int FileId { get; set; }
    public string? CustomName { get; set; }
    public string? Description { get; set; }
    public string? Crc32Hash { get; set; }
    public DateTime FileCreationDateTime { get; set; }
    public DateTime ImageCreatedDateTime { get; set; }
}