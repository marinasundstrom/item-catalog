﻿
using Catalog.ApiKeys.Application.Common.Interfaces;
using Catalog.ApiKeys.Infrastructure.Persistence;
using Catalog.ApiKeys.Infrastructure.Services;

namespace Catalog.ApiKeys.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<ApiKeysContext>(
            configuration.GetConnectionString("mssql", "ApiKeys") ?? configuration.GetConnectionString("DefaultConnection"),
            options => options.EnableRetryOnFailure());

        services.AddScoped<IApiKeysContext>(sp => sp.GetRequiredService<ApiKeysContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}