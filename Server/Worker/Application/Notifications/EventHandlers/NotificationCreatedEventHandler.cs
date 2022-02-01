
using Worker.Application.Common.Interfaces;
using Worker.Application.Common.Models;
using Worker.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Catalog.Application.Common.Interfaces;

namespace Worker.Application.Notifications.EventHandlers;

public class NotificationCreatedEventHandler : INotificationHandler<DomainEventNotification<NotificationCreatedEvent>>
{
    private readonly IWorkerContext _context;
    private readonly INotificationSender _notficationSender;

    public NotificationCreatedEventHandler(IWorkerContext context, INotificationSender notficationSender)
    {
        _context = context;
        _notficationSender = notficationSender;
    }

    public async Task Handle(DomainEventNotification<NotificationCreatedEvent> notification2, CancellationToken cancellationToken)
    {
        var domainEvent = notification2.DomainEvent;

        var notification = await _context.Notifications.FirstAsync(i => i.Id == domainEvent.NotificationId, cancellationToken);

        // Send immediately or schedule

        await _notficationSender.SendNotification(notification);
    }
}
