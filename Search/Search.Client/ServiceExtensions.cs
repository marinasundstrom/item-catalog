using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Search.Client;

public static class ServiceExtensions 
{
    public static IServiceCollection AddSearchClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder> builder) 
    {
        builder(
            services.AddHttpClient(nameof(ISearchClient), configureClient)
            .AddTypedClient<ISearchClient>((http, sp) => new SearchClient(http)));

        return services;
    }
}