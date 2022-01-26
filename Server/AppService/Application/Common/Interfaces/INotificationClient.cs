using Catalog.Application.Notifications;

namespace Catalog.Application.Common.Interfaces;

public interface INotificationClient
{
    Task NotificationReceived(NotificationDto notification);
}
