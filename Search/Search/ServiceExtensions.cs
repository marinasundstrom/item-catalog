using System;

using Catalog.Search.Application;
using Catalog.Search.Application.Common.Interfaces;
using Catalog.Search.Infrastructure;
using Catalog.Search.Services;

namespace Catalog.Search;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}