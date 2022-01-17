using MassTransit;

using Contracts;
using Microsoft.AspNetCore.SignalR;
using Catalog.WebApi.Hubs;
using Catalog.Application.Common.Interfaces;

namespace Worker.Consumers;

public class RandomNotificationConsumer : IConsumer<RandomNotification>
{
    private readonly INotificationClient _notificationClient;

    public RandomNotificationConsumer(INotificationClient notificationClient)
    {
        _notificationClient = notificationClient;
    }

    public async Task Consume(ConsumeContext<RandomNotification> context)
    {
        var message = context.Message;

        await _notificationClient.NotificationReceived(message.Message);
    }
}
