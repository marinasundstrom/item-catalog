using System;

using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<CatalogContext>(configuration.GetConnectionString("mssql"));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogContext>());

        return services;
    }
}

