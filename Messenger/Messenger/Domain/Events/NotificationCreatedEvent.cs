using Catalog.Messenger.Domain.Common;

namespace Catalog.Messenger.Domain.Events;

public class NotificationCreatedEvent : DomainEvent
{
    public NotificationCreatedEvent(string notificationId)
    {
        this.NotificationId = notificationId;
    }

    public string NotificationId { get; }
}