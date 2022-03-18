using System;

using Notifications.Application;
using Notifications.Application.Common.Interfaces;
using Notifications.Infrastructure;
using Notifications.Services;

namespace Notifications;

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