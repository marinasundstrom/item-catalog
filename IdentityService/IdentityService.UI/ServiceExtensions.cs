
using Catalog.IdentityService.Client;
using Catalog.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class ServiceExtensions
{
    public static IServiceCollection AddIdentityUI(this IServiceCollection services)
    {
        services.AddIdentityServiceClients((sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"https://identity.local/");
        }, builder => builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>());

        return services;
    }
}

