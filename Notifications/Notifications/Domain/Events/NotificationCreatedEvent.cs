using Catalog.Notifications.Domain.Common;

namespace Catalog.Notifications.Domain.Events;

public class NotificationCreatedEvent : DomainEvent
{
    public NotificationCreatedEvent(string notificationId)
    {
        this.NotificationId = notificationId;
    }

    public string NotificationId { get; }
}