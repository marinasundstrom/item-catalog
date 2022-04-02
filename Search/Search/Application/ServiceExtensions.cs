
using Catalog.Search.Application.Commands;

using MediatR;

namespace Catalog.Search.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(SearchCommand));

        return services;
    }
}