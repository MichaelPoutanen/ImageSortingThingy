using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ImageSortingThingy.Controls;

public partial class ImageFileListViewControl : UserControl
{
    #region Properties

    // public static readonly StyledProperty<int> NumberOfStarsProperty =
    //     AvaloniaProperty.Register<RatingControl, int>(
    //         nameof(NumberOfStars),          // Sets the name of the property
    //         defaultValue: 5,                // The default value of this property
    //         coerce: CoerceNumberOfStars);   // Ensures that we always have a valid number of stars

    public static readonly StyledProperty<string> StartingDirectory =
        AvaloniaProperty.Register<ImageFileListViewControl, string>(
            name: nameof(StartingDirectory),
            defaultValue: GetStartingDirectory()
        );

    #endregion

    public ImageFileListViewControl()
    {
        InitializeComponent();
    }

    #region Methods

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