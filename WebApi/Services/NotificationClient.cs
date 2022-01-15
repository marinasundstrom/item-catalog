using System;

using Microsoft.AspNetCore.SignalR;

using WebApi.Hubs;

using Catalog.Application.Common.Interfaces;

namespace WebApi.Services;

public class NotificationClient : INotificationClient
{
    private readonly IHubContext<NotificationHub, INotificationClient> _notificationsHubContext;

    public NotificationClient(IHubContext<NotificationHub, INotificationClient> notificationsHubContext)
    {
        _notificationsHubContext = notificationsHubContext;
    }

    public async Task NotificationReceived(string message)
    {
        await _notificationsHubContext.Clients.All.NotificationReceived(message);
    }
}

