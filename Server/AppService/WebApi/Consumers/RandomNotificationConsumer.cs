using Catalog.Application.Common.Interfaces;
using Catalog.WebApi.Hubs;

using Contracts;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

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