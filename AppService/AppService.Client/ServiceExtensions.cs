using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Client;

public static class ServiceExtensions 
{
    public static IServiceCollection AddCatalogClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder> builder) 
    {
        builder(
            services.AddHttpClient(nameof(IClient), configureClient)
            .AddTypedClient<IClient>((http, sp) => new Client(http)));

        builder(
            services.AddHttpClient(nameof(IItemsClient), configureClient)
            .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http)));

        builder(
            services.AddHttpClient(nameof(IDoSomethingClient), configureClient)
            .AddTypedClient<IDoSomethingClient>((http, sp) => new DoSomethingClient(http)));

        builder(
            services.AddHttpClient(nameof(INotificationsClient), configureClient)
            .AddTypedClient<INotificationsClient>((http, sp) => new NotificationsClient(http)));

        return services;
    }
}