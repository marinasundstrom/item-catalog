using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Worker.Client;

public static class ServiceExtensions 
{
    public static IServiceCollection AddWorkerClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder> builder) 
    {
        builder(
            services.AddHttpClient(nameof(IClient), configureClient)
            .AddTypedClient<IClient>((http, sp) => new Client(http)));

        return services;
    }
}