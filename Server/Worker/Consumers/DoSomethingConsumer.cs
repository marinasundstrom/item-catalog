using System;

using Contracts;

using MassTransit;

namespace Worker.Consumers;

public class DoSomethingConsumer : IConsumer<DoSomething>
{
    public async Task Consume(ConsumeContext<DoSomething> context)
    {
        var message = context.Message;

        await Task.Delay(Random.Shared.Next(5000));

        var result = message.LHS + message.RHS;

        await context.Publish(new DoSomethingResponse($"The result is: {result}"));
    }
}