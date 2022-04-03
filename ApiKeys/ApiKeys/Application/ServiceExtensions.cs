
using Catalog.ApiKeys.Application.Commands;

using MediatR;

namespace Catalog.ApiKeys.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(CheckApiKeyCommand));

        return services;
    }
}