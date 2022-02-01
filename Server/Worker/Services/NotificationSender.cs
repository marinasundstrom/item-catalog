using System;

using Catalog.Application.Common.Interfaces;

using Contracts;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Worker.Application.Common.Interfaces;
using Worker.Domain.Entities;

namespace Worker.Services;

public class NotificationSender : INotificationSender
{
    private readonly IBus _bus;
    private readonly IWorkerContext _workerContext;
    private readonly ILogger<NotificationSender> _logger;

    public NotificationSender(IBus bus, IWorkerContext workerContext, ILogger<NotificationSender> logger)
    {
        _bus = bus;
        _workerContext = workerContext;
        _logger = logger;
    }

    public async Task SendNotification(Notification n1)
    {
        _logger.LogInformation("IN SENDER");

        var notification = await _workerContext.Notifications.FirstAsync(n => n.Id == n1.Id);
        notification.Published = DateTime.Now;
        await _workerContext.SaveChangesAsync();

        var notifcationDto = new NotificationDto(notification.Id, notification.Published, notification.Title, notification.Text, notification.Link, notification.UserId, notification.IsRead, notification.Created, notification.CreatedBy, notification.LastModified, notification.LastModifiedBy);

        await _bus.Publish(notifcationDto);
    }
}