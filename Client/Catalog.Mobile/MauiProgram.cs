﻿using System;

using Catalog.Services;

using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

using MudBlazor.Services;

namespace Catalog;

public static class MauiProgram
{
    public const string UriString = $"https://localhost:8080/api/";

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddBlazorWebView();

        builder.Services.AddApp();

        Options.UseNativeUpload = true;

        var services = builder.Services;

        services.Remove(services.First(x => x.ServiceType == typeof(IFilePickerService)));

        services.AddSingleton<IFilePickerService, FilePickerService>();

#if MACCATALYST
        services.Remove(services.First(x => x.ServiceType == typeof(INotificationService)));

        services.AddSingleton<INotificationService, MacCatalyst.NotificationService>();
#endif

        return builder.Build();
    }
}