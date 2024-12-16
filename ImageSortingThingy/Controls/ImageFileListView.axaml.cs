using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ImageSortingThingy.Models;
using ImageSortingThingy.ViewModels;
using ImageSortingThingy.Views;
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
            d(ViewModel!.OpenOptionsDialogInteraction.RegisterHandler(this.OpenOptionsWindowInteractionHandler));
        });
    }

    private async Task OpenOptionsWindowInteractionHandler(
        IInteractionContext<OptionsWindowViewModel, OptionsWindowResponseModel> context)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(this)!;

        OptionsWindow window = new OptionsWindow
        {
            DataContext = context.Input
        };

        OptionsWindowResponseModel result = await window.ShowDialog<OptionsWindowResponseModel>((topLevel as Window)!);
        context.SetOutput(result);
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