﻿using Catalog.Notifications.Client;

namespace Catalog.Application.Common.Interfaces;

public interface INotificationClient
{
    Task NotificationReceived(NotificationDto notification);
}