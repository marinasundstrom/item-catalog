using System;

using Catalog.Notifications.Application;
using Catalog.Notifications.Application.Common.Interfaces;
using Catalog.Notifications.Infrastructure;
using Catalog.Notifications.Services;

namespace Catalog.Notifications;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<INotifier, Notifier>();

        services.AddScoped<INotificationPublisher, NotificationPublisher>();

        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}