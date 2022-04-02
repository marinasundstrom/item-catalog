
using Catalog.Notifications.Application.Notifications.Queries;

using MediatR;

namespace Catalog.Notifications.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(GetNotificationsQuery));

        return services;
    }
}