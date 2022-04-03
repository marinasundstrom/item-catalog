using System;

using Catalog.Application;
using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure;

using Catalog.WebApi.Hubs;
using Catalog.WebApi.Services;

namespace Catalog.WebApi;

public static class ServiceExtensions
{
    public  static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUrlHelper, UrlHelper>();

        services.AddClients();

        services.AddScoped<IFileUploaderService, FileUploaderService>();

        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddScoped<IItemsClient, ItemsClient>();
        services.AddScoped<IWorkerClient, WorkerClient>();
        services.AddScoped<INotificationClient, NotificationClient>();
        services.AddScoped<ISomethingClient, SomethingClient>();

        services.AddHttpClient(nameof(Catalog.IdentityService.Client.IUsersClient) + "2", (sp, http) =>
        {
            http.BaseAddress = new Uri($"https://identity.local/");
            http.DefaultRequestHeaders.Add("X-API-KEY", "asdsr34#34rswert35234aedae?2!");
        })
        .AddTypedClient<Catalog.IdentityService.Client.IUsersClient>((http, sp) => new Catalog.IdentityService.Client.UsersClient(http));

        return services;
    }
}