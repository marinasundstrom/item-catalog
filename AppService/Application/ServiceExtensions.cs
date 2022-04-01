using System;
using System.Net.Http.Headers;

using Catalog.Application.Common.Interfaces;
using Catalog.Application.Items.Queries;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Notifications.Client;

namespace Catalog.Application;

public static class ServiceExtensions
{
    private const string ApiKey = "foobar";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(GetItemsQuery));

        services.AddScoped<Handler>();

        services.AddHttpClient(nameof(INotificationsClient), (sp, http) =>
        {
            var conf = sp.GetRequiredService<IConfiguration>();

            http.BaseAddress = conf.GetServiceUri("notifications");
            http.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        })
        .AddTypedClient<INotificationsClient>((http, sp) => new NotificationsClient(http))
        .AddHttpMessageHandler<Handler>();

        services.AddHttpClient(nameof(SubscriptionGroupsClient), (sp, http) =>
        {
            var conf = sp.GetRequiredService<IConfiguration>();

            http.BaseAddress = conf.GetServiceUri("notifications");
            http.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        })
        .AddTypedClient<ISubscriptionGroupsClient>((http, sp) => new SubscriptionGroupsClient(http))
        .AddHttpMessageHandler<Handler>();

        services.AddHttpClient(nameof(SubscriptionsClient), (sp, http) =>
        {
            var conf = sp.GetRequiredService<IConfiguration>();

            http.BaseAddress = conf.GetServiceUri("notifications");
            http.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
        })
        .AddTypedClient<ISubscriptionsClient>((http, sp) => new SubscriptionsClient(http))
        .AddHttpMessageHandler<Handler>();

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