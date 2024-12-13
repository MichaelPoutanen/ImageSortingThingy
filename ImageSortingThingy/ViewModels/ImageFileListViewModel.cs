using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageSortingThingy.Extensions;
using ImageSortingThingy.Helpers;
using ImageSortingThingy.Models;
using ReactiveUI;

namespace ImageSortingThingy.ViewModels;

public partial class ImageFileListViewModel : ViewModelBase
{
    public ImageFileListViewModel()
    {
        IsInfoLabelVisible = false;
        _errorLevel = 0;

        if (!LoadPreviouslyUsedDirectory())
        {
            CurrentDirectory = GetStartingDirectory();
            InfoLabelText = "Loaded default starting directory.";
        }
        else
        {
            //directory would probably be set in that method, or something
            InfoLabelText = "Loaded last sessions directory.";
        }

        SelectDirectoryCommand = ReactiveCommand.CreateFromTask(SelectDirectory);
        ImagesInDirectorySelectedItem = new ImageFileListEntryModel();
    }

    #region Methods

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

    private void LoadImages()
    {
        List<string> files = SearchImageFiles.GetAllImageFiles(CurrentDirectory).ToList();
        int id = 0;
        foreach (string s in files)
        {
            ImagesInDirectory.Add(s.ToImageFileListEntryModel(id));
            id++;
        }
    }

    private static bool LoadPreviouslyUsedDirectory()
    {
        // This will be implemented in https://github.com/MichaelPoutanen/ImageSortingThingy/issues/2
        return false;
    }

    private static bool SavePreviouslyUsedDirectory(string directory)
    {
        // This will be implemented in https://github.com/MichaelPoutanen/ImageSortingThingy/issues/2
        return false;
    }

    private static string GetStartingDirectory()
    {
        string folder = string.Empty;

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

    private readonly Interaction<string?, string?> _selectDirectoryInteraction = new Interaction<string?, string?>();

    public Interaction<string?, string?> SelectDirectoryInteraction => _selectDirectoryInteraction;

    #endregion

    #region Properties

    private ObservableCollection<ImageFileListEntryModel> _imagesInDirectory = [];

    public ObservableCollection<ImageFileListEntryModel> ImagesInDirectory
    {
        get => _imagesInDirectory;
        set => SetProperty(ref _imagesInDirectory, value);
    }

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