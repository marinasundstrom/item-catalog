
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using Catalog.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Notifications.EventHandlers;

public class NotificationCreatedEventHandler : INotificationHandler<DomainEventNotification<NotificationCreatedEvent>>
{
    private readonly ICatalogContext _context;
    private readonly INotificationClient _notficationClient;

    public NotificationCreatedEventHandler(ICatalogContext context, INotificationClient notficationClient)
    {
        _context = context;
        _notficationClient = notficationClient;
    }

    public async Task Handle(DomainEventNotification<NotificationCreatedEvent> notification2, CancellationToken cancellationToken)
    {
        var domainEvent = notification2.DomainEvent;

        var notification = await _context.Notifications.FirstAsync(i => i.Id == domainEvent.NotificationId, cancellationToken);

        var notifcationDto = new NotificationDto(notification.Id, notification.Published, notification.Title, notification.Text, notification.Link, notification.UserId, notification.IsRead, notification.Created, notification.CreatedBy, notification.LastModified, notification.LastModifiedBy);

        await _notficationClient.NotificationReceived(notifcationDto);
    }
}
