
using AspNetCore.Authentication.ApiKey;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.ApiKeys;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services, string server)
    {
        services.AddHttpClient(nameof(Client.IApiKeysClient), (sp, http) =>
        {
            http.BaseAddress = new Uri(server);
        })
        .AddTypedClient<Catalog.ApiKeys.Client.IApiKeysClient>((http, sp) => new Catalog.ApiKeys.Client.ApiKeysClient(http));

        services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)

            // The below AddApiKeyInHeaderOrQueryParams without type parameter will require options.Events.OnValidateKey delegete to be set.
            //.AddApiKeyInHeaderOrQueryParams(options =>

            // The below AddApiKeyInHeaderOrQueryParams with type parameter will add the ApiKeyProvider to the dependency container. 
            .AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options =>
            {
                options.Realm = "Identity Service API";
                options.KeyName = "X-API-KEY";
            });

        return services;
    }
}

