using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace ImageSortingThingy.Helpers;

public static class MessageBoxHelper
{
    /// <summary>
    /// Returns a Message Box to display an error
    /// </summary>
    /// <param name="errorTitle">Title of the MessageBox</param>
    /// <param name="errorText">Error text (I tend to use the Exception itself)</param>
    /// <returns></returns>
    public static IMsBox<string> ErrorExceptionMessageBox(string errorTitle, string errorText)
    {
        return MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ButtonDefinitions = new List<ButtonDefinition>
            {
                new ButtonDefinition { Name = "Oh snap!", }
            },
            ContentTitle = errorTitle,
            ContentMessage = errorText,
            Icon = MsBox.Avalonia.Enums.Icon.Error,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 600,
            MaxHeight = 800,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = true,
        });
    }
    
    /// <summary>
    /// Used to get a default MessageBox with an "ok" Button and an information icon
    /// </summary>
    /// <param name="infoTitle">Message box title</param>
    /// <param name="infoText">Message box text</param>
    /// <returns></returns>
    public static IMsBox<string> StandardInfoMessageBox(string infoTitle, string infoText)
    {
        return MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ButtonDefinitions = new List<ButtonDefinition>
            {
                new ButtonDefinition { Name = "Ok", },
            },
            ContentTitle = infoTitle,
            ContentMessage = infoText,
            Icon = MsBox.Avalonia.Enums.Icon.Info,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 400,
            MaxHeight = 400,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = true,
        });
    }
    
    /// <summary>
    /// Used to get a Messagebox that contains a Yes and No answer option.
    /// The pressed button will be returned as a string (e.g. "Yes" or "No")
    /// </summary>
    /// <param name="questionTitle">Title of the message box</param>
    /// <param name="questionText">Content of the message box</param>
    /// <returns></returns>
    public static IMsBox<string> StandardYesNoMessageBox(string questionTitle, string questionText)
    {
        return MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ButtonDefinitions = new List<ButtonDefinition>
            {
                new ButtonDefinition { Name = "Yes", },
                new ButtonDefinition { Name = "No", }
            },
            ContentTitle = questionTitle,
            ContentMessage = questionText,
            Icon = MsBox.Avalonia.Enums.Icon.Question,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 400,
            MaxHeight = 400,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = true,
        });
    }
}