using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Notifications.Client;

public static class ServiceExtensions 
{
    public static IServiceCollection AddNotificationsClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder> builder) 
    {
        builder(
            services.AddHttpClient(nameof(INotificationsClient), configureClient)
            .AddTypedClient<INotificationsClient>((http, sp) => new NotificationsClient(http)));

        builder(
            services.AddHttpClient(nameof(ISubscriptionsClient), configureClient)
            .AddTypedClient<ISubscriptionsClient>((http, sp) => new SubscriptionsClient(http)));

        builder(
            services.AddHttpClient(nameof(ISubscriptionGroupsClient), configureClient)
            .AddTypedClient<ISubscriptionGroupsClient>((http, sp) => new SubscriptionGroupsClient(http)));

        return services;
    }
}