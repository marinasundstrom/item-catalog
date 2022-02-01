using System;

using Catalog.Application.Common.Interfaces;

using Contracts;

using MassTransit;

using Worker.Domain.Entities;

namespace Worker.Services;

public class NotificationSender : INotificationSender
{
    private readonly IBus _bus;

    public NotificationSender(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendNotification(Notification notification)
    {
        var notifcationDto = new NotificationDto(notification.Id, notification.Published, notification.Title, notification.Text, notification.Link, notification.UserId, notification.IsRead, notification.Created, notification.CreatedBy, notification.LastModified, notification.LastModifiedBy);

        await _bus.Publish(notifcationDto);
    }
}