using MassTransit;

using MediatR;

using Messenger.Application.Common.Interfaces;
using Messenger.Application.Messages.Commands;
using Messenger.Contracts;

namespace Messenger.Consumers;

public class PostMessageConsumer : IConsumer<PostMessage>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBus _bus;

    public PostMessageConsumer(IMediator mediator, ICurrentUserService currentUserService, IBus bus)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<PostMessage> context)
    {
        var message = context.Message;

        _currentUserService.SetCurrentUser(message.UserId);

        var result = await _mediator.Send(new PostMessageCommand(null!, message.Text, message.ReplyToId));

        await _bus.Publish(new MessagePosted(result));
    }
}

public class UpdateMessageConsumer : IConsumer<UpdateMessage>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBus _bus;

    public UpdateMessageConsumer(IMediator mediator, ICurrentUserService currentUserService, IBus bus)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<UpdateMessage> context)
    {
        var message = context.Message;

        _currentUserService.SetCurrentUser(message.UserId);

        await _mediator.Send(new UpdateMessageCommand(message.MessageId!, message.Text));

        await _bus.Publish(new MessageUpdated(null!, message.MessageId,  message.Text, DateTime.Now));
    }
}

public class DeleteMessageConsumer : IConsumer<DeleteMessage>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBus _bus;

    public DeleteMessageConsumer(IMediator mediator, ICurrentUserService currentUserService, IBus bus)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<DeleteMessage> context)
    {
        var message = context.Message;

        _currentUserService.SetCurrentUser(message.UserId);

        await _mediator.Send(new DeleteMessageCommand(message.MessageId));

        await _bus.Publish(new MessageDeleted(null!, message.MessageId));
    }
}

public class MarkMessageAsReadConsumer : IConsumer<MarkMessageAsRead>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBus _bus;

    public MarkMessageAsReadConsumer(IMediator mediator, ICurrentUserService currentUserService, IBus bus)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<MarkMessageAsRead> context)
    {
        var message = context.Message;

        _currentUserService.SetCurrentUser(message.UserId);

        var receipt = await _mediator.Send(new SendMessageReceiptCommand(message.MessageId));

        await _bus.Publish(new MessageRead(receipt));
    }
}

public class StartTypingConsumer : IConsumer<StartTyping>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public StartTypingConsumer(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task Consume(ConsumeContext<StartTyping> context)
    {
        var message = context.Message;

        _currentUserService.SetCurrentUser(message.UserId);

        //await _mediator.Send(new SendMessageReceiptCommand(message.MessageId));
    }
}

public class EndTypingConsumer : IConsumer<EndedTyping>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public EndTypingConsumer(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task Consume(ConsumeContext<EndedTyping> context)
    {
        var message = context.Message;

        _currentUserService.SetCurrentUser(message.UserId);

        //await _mediator.Send(new SendMessageReceiptCommand(message.MessageId));
    }
}
