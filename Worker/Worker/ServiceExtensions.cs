using Catalog.Worker.Application.Common.Interfaces;
using Catalog.Worker.Services;

namespace Catalog.Worker;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<INotifier, Notifier>();

        services.AddScoped<INotificationPublisher, NotificationPublisher>();

        return services;
    }
}