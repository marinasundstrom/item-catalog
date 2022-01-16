using System;

using Catalog.Application;
using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure;

using WebApi.Hubs;
using WebApi.Services;

namespace WebApi
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
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
            services.AddScoped<INotificationClient, NotificationClient>();
            services.AddScoped<ISomethingClient, SomethingClient>();

            return services;
        }
    }
}

