using Catalog.Application.Common.Interfaces;
using Catalog.WebApi.Hubs;

using Contracts;

using MassTransit;

using Microsoft.AspNetCore.SignalR;

namespace Worker.Consumers;

public class RandomNotificationConsumer : IConsumer<RandomNotification>
{
    private readonly IWorkerlient _workerlient;

    public RandomNotificationConsumer(IWorkerlient workerlient)
    {
        _workerlient = workerlient;
    }

    public async Task Consume(ConsumeContext<RandomNotification> context)
    {
        var message = context.Message;

        await _workerlient.NotificationReceived(message.Message);
    }
}