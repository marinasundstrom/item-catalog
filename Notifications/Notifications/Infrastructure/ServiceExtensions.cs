
using Catalog.Notifications.Application.Common.Interfaces;
using Catalog.Notifications.Infrastructure.Persistence;
using Catalog.Notifications.Infrastructure.Services;

namespace Catalog.Notifications.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<NotificationsContext>(configuration.GetConnectionString("mssql", "Notifications") ?? configuration.GetConnectionString("DefaultConnection"));

        services.AddScoped<INotificationsContext>(sp => sp.GetRequiredService<NotificationsContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}