using System;

using Catalog.IdentityService.Application;
using Catalog.IdentityService.Application.Common.Interfaces;
using Catalog.IdentityService.Infrastructure;
using Catalog.IdentityService.Services;

namespace Catalog.IdentityService;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IEventPublisher, EventPublisher>();

        return services;
    }
}