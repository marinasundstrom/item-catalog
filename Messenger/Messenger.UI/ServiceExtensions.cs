using System;

using Catalog.Messenger.Client;
using Catalog.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Messenger;

public static class ServiceExtensions
{
    public static IServiceCollection AddMessengerUI(this IServiceCollection services)
    {
        services.AddMessengerClients((sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}messenger/");
        }, builder => builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>());

        return services;
    }
}

