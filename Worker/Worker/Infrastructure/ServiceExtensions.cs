
using Catalog.Worker.Application.Common.Interfaces;
using Catalog.Worker.Infrastructure.Persistence;
using Catalog.Worker.Infrastructure.Services;

namespace Catalog.Worker.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<WorkerContext>(
            configuration.GetConnectionString("mssql", "Worker") ?? configuration.GetConnectionString("DefaultConnection"),
            options => options.EnableRetryOnFailure());

        services.AddScoped<IWorkerContext>(sp => sp.GetRequiredService<WorkerContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}