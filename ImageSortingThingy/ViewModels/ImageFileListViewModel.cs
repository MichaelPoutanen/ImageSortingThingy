using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageSortingThingy.Database;
using ImageSortingThingy.Extensions;
using ImageSortingThingy.Helpers;
using ImageSortingThingy.Models;
using ImageSortingThingy.Services;
using ReactiveUI;

namespace ImageSortingThingy.ViewModels;

public partial class ImageFileListViewModel : ViewModelBase
{
    private readonly ImageStorageService _imageStorageService;

    public ImageFileListViewModel( /*ImageStorageService imageStorageService*/)
    {
        _imageStorageService = new ImageStorageService(new ImageToolDbContext());
        IsInfoLabelVisible = false;
        _errorLevel = 0;
        bool autoloadImages = false;

        if (!LoadPreviouslyUsedDirectory())
        {
            CurrentDirectory = GetStartingDirectory();
            InfoLabelText = "Loaded default starting directory.";
        }
        else
        {
            InfoLabelText = "Loaded last sessions directory.";
            if (SettingsHelper.AutomaticallyLoadDataOnStartup)
                autoloadImages = true;
        }

        OpenOptionsWindowCommand = ReactiveCommand.CreateFromTask(OpenOptionsWindow);
        SelectDirectoryCommand = ReactiveCommand.CreateFromTask(SelectDirectory);
        //ImagesInDirectorySelectedItem = new ImageFileListEntryModel();

        if (autoloadImages)
            LoadImages();
    }

    #region Methods

    private async Task OpenOptionsWindow()
    {
        try
        {
            OptionsWindowViewModel vm = new OptionsWindowViewModel();
            OptionsWindowResponseModel result = await OpenOptionsDialogInteraction.Handle(vm);
            SettingsHelper.SetConfigValues(result);
        }
        catch (Exception ex)
        {
#if DEBUG
            Debugger.Break();
#endif
            await MessageBoxHelper.ErrorExceptionMessageBox("Exception in ImageFileListViewModel:OpenOptionsWindow",
                    $"An exception occured while trying to open the options menu.\nError:\n\n{ex}")
                .ShowAsync();
        }
    }

    private async Task SelectDirectory()
    {
        string? selectedDirectory = await _selectDirectoryInteraction.Handle("Test");

        if (!string.IsNullOrEmpty(selectedDirectory))
        {
            if (Path.Exists(selectedDirectory))
            {
                CurrentDirectory = selectedDirectory;

                if (SavePreviouslyUsedDirectory(selectedDirectory))
                {
                    _errorLevel = 1;
                    InfoLabelText = "Successfully selected directory and saved for the next start.";
                }
                else
                {
                    _errorLevel = 0;
                    InfoLabelText = "Successfully selected directory but couldnt save it for future use!";
                }

                LoadImages();
            }
            else
            {
                _errorLevel = 2;
                InfoLabelText = "Directory does not seem exist.";
            }
        }
        else
        {
            _errorLevel = 2;
            InfoLabelText = "Your selection is not valid.";
        }
    }

    private async void LoadImages()
    {
        List<string> files = SearchImageFiles.GetAllImageFiles(CurrentDirectory).ToList();
        List<string> heicFiles = SearchImageFiles.GetAppleImageFiles(CurrentDirectory).ToList();
        int id = 0;

        if (heicFiles.Count > 0)
        {
            //Todo: We should track the converted heic files via Database, CRCing the heic file to prevent converting 
            //      the same files over and over again
            string answer = await MessageBoxHelper.StandardYesNoMessageBox("Apple Image files found",
                "Apple image files (heic) have been found.\n They are normally not supported, but we can" +
                "convert them to jpg Images automatically.\n"
                + "(The Original files will not be touched!\n" +
                "Do you want to convert them to jpg?").ShowAsync();

            if (answer.ToLower().Equals("yes"))
            {
                foreach (string heicFile in heicFiles)
                {
                    //Place them in the same directory, just with .jpg instead of .heic at the end
                    ImageFormatConverter.ConvertHeicToJpg(heicFile, heicFile.Replace(".heic", ".jpg"));
                }
            }
        }

        foreach (string s in files)
        {
            ImageFileListEntryModel model = s.ToImageFileListEntryModel(id);

            try
            {
                if (!_imageStorageService.DoesImageExistInDatabase(model.Crc32Hash!))
                {
                    _imageStorageService.AddImage(model);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debugger.Break();
#endif
                await MessageBoxHelper.ErrorExceptionMessageBox("Exception in ImageFileListViewModel:LoadImages",
                        $"An exception occured while trying to load images.\nError:\n\n{ex}")
                    .ShowAsync();
            }


            ImagesInDirectory.Add(s.ToImageFileListEntryModel(id));
            id++;
        }

        await MessageBoxHelper.StandardInfoMessageBox("Successfully loaded images", $"Loaded {id} images.").ShowAsync();
    }

    private bool LoadPreviouslyUsedDirectory()
    {
        string storedValue = SettingsHelper.WorkingDirectory;

        if (!string.IsNullOrWhiteSpace(storedValue))
        {
            CurrentDirectory = storedValue;
            return true;
        }

        return false;
    }

    private static bool SavePreviouslyUsedDirectory(string directory)
    {
        try
        {
            SettingsHelper.WorkingDirectory = directory;
            return true;
        }
        catch (Exception e)
        {
#if DEBUG
            Console.WriteLine(e);
            Debugger.Break();
            throw;
#endif
            //The warning here has been disabled because the code is reachable in Release mode.
#pragma warning disable CS0162 // Unreachable code detected
            return false;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }

    private static string GetStartingDirectory()
    {
        string folder;

        if (!string.IsNullOrWhiteSpace(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
            folder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        else if (!string.IsNullOrWhiteSpace(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures)))
            folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
        else if (!string.IsNullOrWhiteSpace(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
            folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        else if (!string.IsNullOrWhiteSpace(Environment.GetFolderPath(Environment.SpecialFolder.Personal)))
            folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        else if (!string.IsNullOrWhiteSpace(Environment.GetFolderPath(Environment.SpecialFolder.Windows)))
            folder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        else if (!string.IsNullOrWhiteSpace(Environment.GetFolderPath(Environment.SpecialFolder.System)))
            folder = Environment.GetFolderPath(Environment.SpecialFolder.System);
        else
            throw new SystemException(
                "Exception! Your system doesn't seem to have any Special Folders (like root or user folders) defined. Terminating execution.");

        return folder;
    }

    #endregion

    #region Commands

    public ICommand SelectDirectoryCommand { get; }

    public ICommand OpenOptionsWindowCommand { get; }

    private readonly Interaction<string?, string?> _selectDirectoryInteraction = new();

    private readonly Interaction<OptionsWindowViewModel, OptionsWindowResponseModel> _openOptionsDialogInteraction =
        new();

    public Interaction<string?, string?> SelectDirectoryInteraction => _selectDirectoryInteraction;

    public Interaction<OptionsWindowViewModel, OptionsWindowResponseModel> OpenOptionsDialogInteraction =>
        _openOptionsDialogInteraction;

    #endregion

    #region Properties

    private ObservableCollection<ImageFileListEntryModel> _imagesInDirectory = [];

    public ObservableCollection<ImageFileListEntryModel> ImagesInDirectory
    {
        get => _imagesInDirectory;
        set => SetProperty(ref _imagesInDirectory, value);
    }

    // Used in the view, thus public; Naming is like that because it will generate the uppercase property
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBePrivate.Global
    [ObservableProperty] public ImageFileListEntryModel _imagesInDirectorySelectedItem;

    private IBrush _infoLabelBrush = Brushes.Transparent;

    public IBrush InfoLabelBrush
    {
        get => _infoLabelBrush;
        set => SetProperty(ref _infoLabelBrush, value);
    }

    private bool _isInfoLabelVisible;

    public bool IsInfoLabelVisible
    {
        get => _isInfoLabelVisible;
        set => SetProperty(ref _isInfoLabelVisible, value);
    }

    /// <summary>
    /// Contains information about the current Information level.
    /// 0: Nothing special happened, no information. Brush colour transparent for prod, Black in debug 
    /// 1: Success, is coloured in green
    /// 2: Error, is coloured in red
    /// </summary>
    private int _errorLevel;

    private string _infoLabelText = string.Empty;

    public string InfoLabelText
    {
        get => _infoLabelText;
        set
        {
            SetProperty(ref _infoLabelText, value);

            if (_errorLevel == 0)
            {
                //Used for information messages
                InfoLabelBrush = Brushes.Yellow;
                IsInfoLabelVisible = true;
            }
            else if (_errorLevel == 1)
            {
                //color = green, something worked out
                InfoLabelBrush = Brushes.DarkGreen;
                IsInfoLabelVisible = true;
            }
            else if (_errorLevel == 2)
            {
                //color = red, error
                InfoLabelBrush = Brushes.Red;
                IsInfoLabelVisible = true;
            }
            else
            {
                InfoLabelBrush = Brushes.Transparent;
                IsInfoLabelVisible = false;
            }
        }
    }

    private string _currentDirectory = string.Empty;

    public string CurrentDirectory
    {
        get => _currentDirectory;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (_currentDirectory.Equals(value))
                {
                    _errorLevel = 0;
                    InfoLabelText = "This directory is already selected.";
                }
                else
                {
                    if (SetProperty(ref _currentDirectory, value))
                    {
                        _errorLevel = 1;
                        InfoLabelText = "Directory successfully selected";
                    }
                    else
                    {
                        _errorLevel = 2;
                        InfoLabelText = "Error setting the _currentDirectory Property in SetProperty";
                    }
                }
            }
            else
            {
                _currentDirectory = string.Empty;
                _errorLevel = 2;
                InfoLabelText = "Error: Invalid Directory selected!";
            }
        }
    }

    #endregion
}