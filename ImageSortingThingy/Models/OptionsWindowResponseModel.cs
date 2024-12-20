namespace ImageSortingThingy.Models;

public class OptionsWindowResponseModel
{
    public bool AlwaysSaveAddedImages { get; set; }
    public bool AutomaticallyLoadDataOnStartup { get; set; }
    public string ThumbnailStoragePath { get; set; } = string.Empty;
}