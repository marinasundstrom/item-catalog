using Catalog.Worker.Domain.Common;

namespace Catalog.Worker.Domain.Events;

public class NotificationCreatedEvent : DomainEvent
{
    public NotificationCreatedEvent(string notificationId)
    {
        this.NotificationId = notificationId;
    }

    public string NotificationId { get; }
}