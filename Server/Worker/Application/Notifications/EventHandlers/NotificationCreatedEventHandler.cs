
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
    private readonly IServiceProvider _serviceProvider;
    private readonly INotificationSender _notficationSender;
    private readonly IBackgroundJobClient _recurringJobManager;

    public NotificationCreatedEventHandler(IServiceProvider serviceProvider, INotificationSender notficationSender, IBackgroundJobClient recurringJobManager)
    {
        _serviceProvider = serviceProvider;
        _notficationSender = notficationSender;
        _recurringJobManager = recurringJobManager;
    }

    public async Task Handle(DomainEventNotification<NotificationCreatedEvent> notification2, CancellationToken cancellationToken)
    {
        var domainEvent = notification2.DomainEvent;

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IWorkerContext>();

        var notification = await context.Notifications.FirstAsync(i => i.Id == domainEvent.NotificationId, cancellationToken);

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

            notification.ScheduledJobId = jobId;

            await context.SaveChangesAsync();
        }
        else
        {
            await _notficationSender.SendNotification(notification);
        }
    }
}
