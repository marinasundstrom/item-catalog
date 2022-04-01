using System;

using Catalog.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class ServiceExtensions
{
    public static IServiceCollection AddIdentityUI(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(Catalog.IdentityService.Client.IUsersClient) + "2", (sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"https://identity.local/");
        })
        .AddTypedClient<Catalog.IdentityService.Client.IUsersClient>((http, sp) => new Catalog.IdentityService.Client.UsersClient(http))
        .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        services.AddHttpClient(nameof(Catalog.IdentityService.Client.IRolesClient) + "2", (sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"https://identity.local/");
        })
        .AddTypedClient<Catalog.IdentityService.Client.IRolesClient>((http, sp) => new Catalog.IdentityService.Client.RolesClient(http))
        .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        return services;
    }
}

