using MassTransit;

using MediatR;

using Messenger.Application.Common.Interfaces;
using Messenger.Contracts;
using Messenger.Hubs;

using Microsoft.AspNetCore.SignalR;

namespace Messenger.Consumers;

public class MessagePostedConsumer : IConsumer<MessagePosted>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHubContext<MessageHub, IMessageClient> _hubContext;

    public MessagePostedConsumer(IMediator mediator, ICurrentUserService currentUserService,
        IHubContext<MessageHub, IMessageClient> hubContext)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<MessagePosted> context)
    {
        var message = context.Message;

        await _hubContext.Clients.All.MessageReceived(message.Message);
    }
}

public class MessageUpdatedConsumer : IConsumer<MessageUpdated>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHubContext<MessageHub, IMessageClient> _hubContext;

    public MessageUpdatedConsumer(IMediator mediator, ICurrentUserService currentUserService,
        IHubContext<MessageHub, IMessageClient> hubContext)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<MessageUpdated> context)
    {
        var message = context.Message;

        await _hubContext.Clients.All.MessageEdited(new MessageEditedDto()
        {
            Id = message.MessageId,
            Text = message.Text,
            Edited = message.Edited
        });
    }
}

public class MessageDeletedConsumer : IConsumer<MessageDeleted>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHubContext<MessageHub, IMessageClient> _hubContext;

    public MessageDeletedConsumer(IMediator mediator, ICurrentUserService currentUserService,
        IHubContext<MessageHub, IMessageClient> hubContext)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<MessageDeleted> context)
    {
        var message = context.Message;

        await _hubContext.Clients.All.MessageDeleted(new MessageDeletedDto()
        {
            Id = message.MessageId
        });
    }
}

public class MessageReadConsumer : IConsumer<MessageRead>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHubContext<MessageHub, IMessageClient> _hubContext;

    public MessageReadConsumer(IMediator mediator, ICurrentUserService currentUserService,
        IHubContext<MessageHub, IMessageClient> hubContext)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<MessageRead> context)
    {
        var message = context.Message;

        await _hubContext.Clients.All.MessageRead(message.Receipt!);
    }
}

//StartedTyping
//EndedTyping