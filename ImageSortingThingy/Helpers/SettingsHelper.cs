using System;
using System.Configuration;
using System.Diagnostics;
using ImageSortingThingy.Models;

// ReSharper disable InvertIf

namespace ImageSortingThingy.Helpers;

public static class SettingsHelper
{
    private static readonly Configuration Config =
        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

    public static bool AlwaysSaveAddedImages
    {
        get => GetSetting(nameof(AlwaysSaveAddedImages), true);
        set => SetSetting(nameof(AlwaysSaveAddedImages), value);
    }

    public static bool AutomaticallyLoadDataOnStartup
    {
        get => GetSetting(nameof(AutomaticallyLoadDataOnStartup), true);
        set => SetSetting(nameof(AutomaticallyLoadDataOnStartup), value);
    }

    public static string ThumbnailStoragePath
    {
        get => GetSetting(nameof(ThumbnailStoragePath), GlobalDefinitions.ThumbnailStoragePath);
        set => SetSetting(nameof(ThumbnailStoragePath), value);
    }

    #region Methods

    public static string GetConfigurationFilePath()
    {
        return Config.FilePath;
    }

    public static void SetConfigValues(OptionsWindowResponseModel responseModel)
    {
        AlwaysSaveAddedImages = responseModel.AlwaysSaveAddedImages;
        AutomaticallyLoadDataOnStartup = responseModel.AutomaticallyLoadDataOnStartup;
        ThumbnailStoragePath = responseModel.ThumbnailStoragePath;
    }

    private static T GetSetting<T>(string key, T defaultValue)
    {
        if (Config.AppSettings.Settings[key] != null && Config.AppSettings.Settings[key].Value is { } value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        return defaultValue;
    }

    private static void SetSetting<T>(string key, T value)
    {
        if (Config.AppSettings.Settings[key] == null)
        {
            Config.AppSettings.Settings.Add(key, value?.ToString());
        }
        else
        {
            Config.AppSettings.Settings[key].Value = value?.ToString();
        }

        Config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
    }

    #endregion
}