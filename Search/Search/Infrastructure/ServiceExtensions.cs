using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Catalog.Search.Application.Common.Interfaces;
using Catalog.Search.Infrastructure.Persistence;
using Catalog.Search.Infrastructure.Services;

namespace Catalog.Search.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<SearchContext>(configuration.GetConnectionString("mssql", "Search") ?? configuration.GetConnectionString("DefaultConnection"));

        services.AddScoped<ISearchContext>(sp => sp.GetRequiredService<SearchContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}