using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using ImageSortingThingy.ViewModels;
using ImageSortingThingy.Views;

namespace ImageSortingThingy;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // ServiceCollection collection = new();
        // collection.AddCommonServices();
        // ServiceProvider services=collection.BuildServiceProvider();
        // MainWindowViewModel vm = services.GetRequiredService<MainWindowViewModel>();
        //
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            desktop.MainWindow = new MainWindow
            {
                DataContext = /*vm*/new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}