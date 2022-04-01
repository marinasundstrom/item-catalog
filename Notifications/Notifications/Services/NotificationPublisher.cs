using System;

using Contracts;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Notifications.Application.Common.Interfaces;
using Notifications.Domain.Entities;

namespace Notifications.Services;

public class NotificationPublisher : INotificationPublisher
{
    private readonly IBus _bus;
    private readonly IEmailService _emailService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationPublisher> _logger;

    public NotificationPublisher(IBus bus, IServiceProvider serviceProvider, ILogger<NotificationPublisher> logger)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task PublishNotification(Notification n1)
    {
        _logger.LogInformation("Sending notification");

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<INotificationsContext>();

        var notification = await context.Notifications
            .Include(n => n.User)
            .FirstAsync(n => n.Id == n1.Id);

        notification.Published = DateTime.Now;
        await context.SaveChangesAsync();

        var notifcationDto = new NotificationDto(notification.Id, notification.Published, notification.Title, notification.Text, notification.Link, notification.UserId, notification.IsRead, notification.Created, notification.CreatedById, notification.LastModified, notification.LastModifiedById);

        await _bus.Publish(notifcationDto);

        await SendMail(scope, notification);
    }

    private static async Task SendMail(IServiceScope scope, Notification notification)
    {
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        await emailService.Send(notification.User.Email, notification.Title, notification.Text);
    }
}