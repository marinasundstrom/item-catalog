using Catalog.Messenger.Application.Messages.Queries;

using MediatR;


namespace Catalog.Messenger.Application;

public static class ServiceExtensions
{
    private const string ApiKey = "foobar";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(GetMessagesIncrQuery));

        /*
        services.AddHttpClient(nameof(INotificationsClient), (sp, http) =>
        {
            var conf = sp.GetRequiredService<IConfiguration>();

            http.BaseAddress = conf.GetServiceUri("notifications");
            http.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        })
        .AddTypedClient<INotificationsClient>((http, sp) => new NotificationsClient(http))
        .AddHttpMessageHandler<Handler>();
        */

        return services;
    }
}