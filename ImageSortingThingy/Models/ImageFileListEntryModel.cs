using System;

namespace ImageSortingThingy.Models;

public class ImageFileListEntryModel
{
    public string? FileName { get; set; }
    public string? AbsolutePath { get; set; }
    public int FileId { get; set; }
    
    public string? CustomName { get; set; }
    public string? Description { get; set; }
    public DateTime FileCreationDateTime { get; set; }
    public DateTime ImageCreatedDateTime { get; set; }
}