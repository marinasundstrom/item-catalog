using System;

using Catalog.Application.Common.Interfaces;
using Catalog.Application.Notifications;
using Catalog.WebApi.Hubs;

using Microsoft.AspNetCore.SignalR;

namespace Catalog.WebApi.Services;

public class NotificationClient : INotificationClient
{
    private readonly IHubContext<NotificationHub, INotificationClient> _notificationsHubContext;

    public NotificationClient(IHubContext<NotificationHub, INotificationClient> notificationsHubContext)
    {
        _notificationsHubContext = notificationsHubContext;
    }

    public async Task NotificationReceived(NotificationDto notification)
    {
        await _notificationsHubContext.Clients.All.NotificationReceived(notification);
    }
}
