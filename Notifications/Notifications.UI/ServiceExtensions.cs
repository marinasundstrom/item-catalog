using System;

using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Notifications;

public static class ServiceExtensions
{
    public static IServiceCollection AddNotificationsUI(this IServiceCollection services)
    {
        return services;
    }
}

