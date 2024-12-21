using System;

namespace ImageSortingThingy.Models;

public class ConvertedFileModel
{
    public int Id { get; set; }
    //Navigation property
    public ImageFileListEntryModel? ImageFileModel { get; set; }
    public required string OriginalFilePath { get; set; }
    public required string OriginalFileHash { get; set; }
    public bool IsConverted { get; set; }
    public string? ConvertedFilePath { get; set; }
    public string? ConvertedFileHash { get; set; }
    public DateTime ConvertedDateTime { get; set; }
}