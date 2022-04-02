using System;

using Catalog.Shared.Authorization;
using Catalog.Shared.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Shared;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}

