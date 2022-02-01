
using Worker.Application.Common.Interfaces;
using Worker.Application.Common.Models;
using Worker.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Catalog.Application.Common.Interfaces;
using Hangfire;

namespace Worker.Application.Notifications.EventHandlers;

public class NotificationCreatedEventHandler : INotificationHandler<DomainEventNotification<NotificationCreatedEvent>>
{
    private readonly IWorkerContext _context;
    private readonly INotificationSender _notficationSender;
    private readonly IBackgroundJobClient _recurringJobManager;

    public NotificationCreatedEventHandler(IWorkerContext context, INotificationSender notficationSender, IBackgroundJobClient recurringJobManager)
    {
        _context = context;
        _notficationSender = notficationSender;
        _recurringJobManager = recurringJobManager;
    }

    public async Task Handle(DomainEventNotification<NotificationCreatedEvent> notification2, CancellationToken cancellationToken)
    {
        var domainEvent = notification2.DomainEvent;

        var notification = await _context.Notifications.FirstAsync(i => i.Id == domainEvent.NotificationId, cancellationToken);

        if (notification.ScheduledFor is not null)
        {
            if(notification.ScheduledFor < DateTime.UtcNow)
            {
                throw new Exception();
            }

            var offset = notification.ScheduledFor.GetValueOrDefault() - DateTime.UtcNow;

            var jobId = _recurringJobManager.Schedule<INotificationSender>(
                (sender) => sender.SendNotification(notification),
                    offset);
        }
        else
        {
            await _notficationSender.SendNotification(notification);
        }
    }
}
