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