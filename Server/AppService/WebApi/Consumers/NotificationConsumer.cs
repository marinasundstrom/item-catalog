using Catalog.Application.Common.Interfaces;
using Catalog.WebApi.Hubs;

using Contracts;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

namespace Catalog.Consumers;

public class NotificationConsumer : IConsumer<NotificationDto>
{
    private readonly INotificationClient _notificationClient;

    public NotificationConsumer(INotificationClient notificationClient)
    {
        _notificationClient = notificationClient;
    }

    public async Task Consume(ConsumeContext<NotificationDto> context)
    {
        var message = context.Message;

        var dto = new Worker.Client.NotificationDto();

        await _notificationClient.NotificationReceived(dto);
    }
}
