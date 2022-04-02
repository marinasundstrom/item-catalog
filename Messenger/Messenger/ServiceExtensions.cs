using Catalog.Messenger.Application.Common.Interfaces;
using Catalog.Messenger.Services;

namespace Catalog.Messenger;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}