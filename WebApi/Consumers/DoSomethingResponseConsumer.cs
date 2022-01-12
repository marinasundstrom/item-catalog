using System;
using MassTransit;

using Contracts;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace Worker.Consumers;

public class DoSomethingResponseConsumer : IConsumer<DoSomethingResponse>
{
    private readonly IHubContext<SomethingHub, ISomethingClient> _hubContext;

    public DoSomethingResponseConsumer(IHubContext<SomethingHub, ISomethingClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<DoSomethingResponse> context)
    {
        var message = context.Message;

        await _hubContext.Clients.All.ResponseReceived(message.Message);
    }
}
