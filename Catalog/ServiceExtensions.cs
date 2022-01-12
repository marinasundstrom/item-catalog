using System;

using Catalog.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

using MudBlazor.Services;

namespace Catalog;

public static class ServiceExtensions
{
    public static IServiceCollection AddApp(this IServiceCollection services)
    {
        services.AddMudServices();

        services.AddScoped<INotificationService, NotificationService>();

        services.AddSingleton<IFilePickerService, FilePickerService>();

        services.AddHttpClient(nameof(Catalog.Client.IClient), (sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}/api/");
        })
        .AddTypedClient<Catalog.Client.IClient>((http, sp) => new Catalog.Client.Client(http));

        services.AddHttpClient(nameof(Catalog.Client.IItemsClient), (sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}/api/");
        })
        .AddTypedClient<Catalog.Client.IItemsClient>((http, sp) => new Catalog.Client.ItemsClient(http));

        services.AddHttpClient(nameof(Catalog.Client.IDoSomethingClient), (sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}/api/");
        })
        .AddTypedClient<Catalog.Client.IDoSomethingClient>((http, sp) => new Catalog.Client.DoSomethingClient(http));

        return services;
    }
}

