using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Catalog.Services;
using System;
using MudBlazor.Services;

namespace Catalog
{
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

            builder.Services.AddMudServices();

            builder.Services.AddHttpClient(nameof(Catalog.Client.IClient), (sp, http) =>
            {
                http.BaseAddress = new Uri(UriString);
            })
            .AddTypedClient<Catalog.Client.IClient>((http, sp) => new Catalog.Client.Client(http));

            builder.Services.AddHttpClient(nameof(Catalog.Client.IItemsClient), (sp, http) =>
            {
                http.BaseAddress = new Uri(UriString);
            })
            .AddTypedClient<Catalog.Client.IItemsClient>((http, sp) => new Catalog.Client.ItemsClient(http));

            var services = builder.Services;
#if WINDOWS
            services.AddSingleton<INotificationService, WinUI.NotificationService>();
#elif MACCATALYST
            services.AddSingleton<INotificationService, MacCatalyst.NotificationService>();
#elif IOS
            services.AddScoped<INotificationService, iOS.NotificationService>();
#elif ANDROID
            services.AddScoped<INotificationService, Droid.NotificationService>();
#endif

            return builder.Build();
        }
    }
}
