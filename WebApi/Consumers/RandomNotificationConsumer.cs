using MassTransit;

using Contracts;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace Worker.Consumers;

public class RandomNotificationConsumer : IConsumer<RandomNotification>
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

    public RandomNotificationConsumer(IHubContext<NotificationHub, INotificationClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<RandomNotification> context)
    {
        var message = context.Message;

        await _hubContext.Clients.All.NotificationReceived(message.Message);
    }
}
