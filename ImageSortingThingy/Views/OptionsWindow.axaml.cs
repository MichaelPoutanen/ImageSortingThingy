using Avalonia.ReactiveUI;
using ImageSortingThingy.Models;
using ImageSortingThingy.ViewModels;

namespace ImageSortingThingy.Views;

public partial class OptionsWindow : ReactiveWindow<OptionsWindowViewModel>
{
    public OptionsWindow()
    {
        InitializeComponent();

        // this.WhenActivated(action => action(ViewModel!.SaveAndCloseCommand.Subscribe(Close)));
        DataContextChanged += (s, e) =>
        {
            if (DataContext is OptionsWindowViewModel vm)
            {
                vm.RequestClose += OnRequestClose;
            }
        };
    }

    #region Methods

    private void OnRequestClose(OptionsWindowResponseModel response)
    {
        Close(response);
    }

    #endregion
}