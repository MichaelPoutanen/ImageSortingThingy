using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;

namespace ImageSortingThingy.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
    }

    public ImageFileListViewModel ImageFileListViewModel { get; } = new();
}