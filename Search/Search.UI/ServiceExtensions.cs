
using Catalog.Search.Client;
using Catalog.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Search;

public static class ServiceExtensions
{
    public static IServiceCollection AddSearchUI(this IServiceCollection services)
    {
        services.AddSearchClient((sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}search/");
        }, builder => builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>());

        return services;
    }
}

