using System;

using Messenger.Application;
using Messenger.Application.Common.Interfaces;
using Messenger.Infrastructure;
using Messenger.Services;

namespace Messenger;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}