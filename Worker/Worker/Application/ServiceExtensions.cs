
using MediatR;

namespace Catalog.Worker.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(ServiceExtensions));

        return services;
    }
}