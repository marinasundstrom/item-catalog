
using Catalog.Notifications.Client;
using Catalog.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Notifications;

public static class ServiceExtensions
{
    public static IServiceCollection AddNotificationsUI(this IServiceCollection services)
    {
        /*
         
        services.AddNotificationsClients((sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}notifications/");
        }, builder => builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>());

        */

        return services;
    }
}

