using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ImageSortingThingy.ViewModels;
using ReactiveUI;

namespace ImageSortingThingy.Controls;

public partial class ImageFileListView : ReactiveUserControl<ImageFileListViewModel>
{
    public ImageFileListView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            d(ViewModel!.SelectDirectoryInteraction.RegisterHandler(this.SelectDirectoryInteractionHandler));
        });
    }
    
    private async Task SelectDirectoryInteractionHandler(IInteractionContext<string?, string?> context)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;

        IReadOnlyList<IStorageFolder> storageFiles = await topLevel!.StorageProvider
            .OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                //SuggestedStartLocation = context.Input,
                Title = "Select the root folder of your pictures"
            });
         
        context.SetOutput(storageFiles.FirstOrDefault()?.Path.LocalPath);
    }
}