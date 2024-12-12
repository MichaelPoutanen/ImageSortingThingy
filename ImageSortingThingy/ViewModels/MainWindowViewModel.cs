using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Media;
using ReactiveUI;

namespace ImageSortingThingy.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        IsInfoLabelVisible = false;
        _errorLevel = 0;
        
        if(!LoadPreviouslyUsedDirectory())
        { 
            CurrentDirectory = GetStartingDirectory();
            InfoLabelText = "Loaded default starting directory.";
        }
        else
        {
            //directory would probably be set in that method, or something
            InfoLabelText = "Loaded last sessions directory.";
        }
        SelectDirectoryCommand = ReactiveCommand.Create(SelectDirectory);
    }

    #region Commands

    public ICommand SelectDirectoryCommand { get; }

    private void SelectDirectory()
    {

    }

    #endregion

    #region Properties

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

    #region Methods

    private static bool LoadPreviouslyUsedDirectory()
    {
        // Todo: This needs to be implemented
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
}