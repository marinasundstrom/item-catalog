using System;
using MassTransit;

using Contracts;
using Microsoft.AspNetCore.SignalR;
using Catalog.WebApi;
using Catalog.Application.Common.Interfaces;

namespace Worker.Consumers;

public class DoSomethingResponseConsumer : IConsumer<DoSomethingResponse>
{
    private readonly ISomethingClient _somethingClient;

    public DoSomethingResponseConsumer(ISomethingClient somethingClient)
    {
        _somethingClient = somethingClient;
    }

    public async Task Consume(ConsumeContext<DoSomethingResponse> context)
    {
        var message = context.Message;

        await _somethingClient.ResponseReceived(message.Message);
    }
}
