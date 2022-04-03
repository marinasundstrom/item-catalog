using Catalog.ApiKeys.Application.Common.Interfaces;
using Catalog.ApiKeys.Services;

namespace Catalog.ApiKeys;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}