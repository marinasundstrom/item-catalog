using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IdentityService.Client;

public static class ServiceExtensions 
{
    public static IServiceCollection AddIdentityServiceClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddUsersClient(configureClient, builder)
            .AddRolesClient(configureClient, builder);

        return services;
    }

    public static IServiceCollection AddUsersClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(IUsersClient), configureClient)
            .AddTypedClient<IUsersClient>((http, sp) => new UsersClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddRolesClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b =
            services.AddHttpClient(nameof(IRolesClient), configureClient)
            .AddTypedClient<IRolesClient>((http, sp) => new RolesClient(http));

        builder?.Invoke(b);

        return services;
    }
}