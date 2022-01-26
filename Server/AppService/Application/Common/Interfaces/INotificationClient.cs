using Catalog.Application.Notifications;

namespace Catalog.Application.Common.Interfaces;

public interface INotificationClient
{
    Task NotificationReceived(string message);
    Task NotificationReceived2(NotificationDto notification);
}