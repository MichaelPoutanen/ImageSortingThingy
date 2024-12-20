namespace ImageSortingThingy.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
    }

    public ImageFileListViewModel ImageFileListViewModel { get; } = new();
}