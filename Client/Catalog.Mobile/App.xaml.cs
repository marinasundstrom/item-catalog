using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;

using Application = Microsoft.Maui.Controls.Application;

namespace Catalog;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }

    protected override void OnStart()
    {
        base.OnStart();

        //await _services.ConfigureApp();
    }
}