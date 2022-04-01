using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Notifications.Application.Common.Interfaces;
using Notifications.Infrastructure.Persistence;
using Notifications.Infrastructure.Services;

namespace Notifications.Infrastructure;

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