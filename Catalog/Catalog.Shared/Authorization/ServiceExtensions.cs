using System;

using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Shared.Authorization;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}

