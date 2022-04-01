﻿using System;

using Catalog.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Messenger;

public static class ServiceExtensions
{
    public static IServiceCollection AddMessengerUI(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(global::Messenger.Client.IConversationsClient), (sp, http) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            http.BaseAddress = new Uri($"{navigationManager.BaseUri}messenger/");
        })
        .AddTypedClient<global::Messenger.Client.IConversationsClient>((http, sp) => new global::Messenger.Client.ConversationsClient(http))
        .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        return services;
    }
}
