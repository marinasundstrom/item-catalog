using System;

using Catalog.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Search;

public static class ServiceExtensions
{
    public static IServiceCollection AddSearchUI(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(Client.ISearchClient), (sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}search/");
        })
        .AddTypedClient<Client.ISearchClient>((http, sp) => new Client.SearchClient(http))
        .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        return services;
    }
}

