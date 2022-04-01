﻿using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Messenger.Application.Common.Interfaces;
using Messenger.Infrastructure.Persistence;
using Messenger.Infrastructure.Services;

namespace Messenger.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<MessengerContext>(configuration.GetConnectionString("mssql", "Messenger") ?? configuration.GetConnectionString("DefaultConnection"));

        services.AddScoped<IMessengerContext>(sp => sp.GetRequiredService<MessengerContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}