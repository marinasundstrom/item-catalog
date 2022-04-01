﻿using System;
using System.Net.Http.Headers;

using Messenger.Application.Common.Interfaces;
using Messenger.Application.Messages.Queries;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Messenger.Application;

public static class ServiceExtensions
{
    private const string ApiKey = "foobar";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(GetMessagesIncrQuery));

        services.AddScoped<Handler>();

        /*
        services.AddHttpClient(nameof(INotificationsClient), (sp, http) =>
        {
            var conf = sp.GetRequiredService<IConfiguration>();

            http.BaseAddress = conf.GetServiceUri("notifications");
            http.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        })
        .AddTypedClient<INotificationsClient>((http, sp) => new NotificationsClient(http))
        .AddHttpMessageHandler<Handler>();
        */

        return services;
    }
}

public class Handler : DelegatingHandler
{
    private readonly ICurrentUserService _currentUserService;

    public Handler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _currentUserService.GetAccessToken());

        return base.SendAsync(request, cancellationToken);
    }
}