using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageSortingThingy.Helpers;
using ImageSortingThingy.Models;
using ReactiveUI;

namespace ImageSortingThingy.ViewModels;

public partial class OptionsWindowViewModel : ViewModelBase
{
    public OptionsWindowViewModel()
    {
       LoadResponseModelData();
       SetResponseModelData();
        //SaveAndCloseCommand=ReactiveCommand.Create(() => ResponseModel);
        SaveAndCloseCommand = new RelayCommand(SaveAndClose);
    }
    
    #region Properties

    private OptionsWindowResponseModel ResponseModel { get; } = new();

    public event Action<OptionsWindowResponseModel>? RequestClose;
    
    [ObservableProperty] private bool _alwaysSaveAddedImages;
    [ObservableProperty] private bool _automaticallyLoadDataOnStartup;
    [ObservableProperty] private string _thumbnailStoragePath = string.Empty;
    
    
    #endregion
    
    #region Methods

    private void SaveAndClose()
    {
        SetResponseModelData();
        RequestClose?.Invoke(ResponseModel);
    }

    private void LoadResponseModelData()
    {
        AlwaysSaveAddedImages = SettingsHelper.AlwaysSaveAddedImages;
        AutomaticallyLoadDataOnStartup=SettingsHelper.AutomaticallyLoadDataOnStartup;
        ThumbnailStoragePath=SettingsHelper.ThumbnailStoragePath;
    }

    private void SetResponseModelData()
    {
        ResponseModel.AlwaysSaveAddedImages = AlwaysSaveAddedImages;
        ResponseModel.AutomaticallyLoadDataOnStartup = AutomaticallyLoadDataOnStartup;
        ResponseModel.ThumbnailStoragePath = ThumbnailStoragePath;
    }
    
    #endregion
    
    #region Commands
    
    public ICommand SaveAndCloseCommand { get; }
    
    #endregion
}