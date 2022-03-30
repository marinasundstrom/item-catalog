
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Messages;
using Messenger.Application.Messages.Commands;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Hubs;

[Authorize(AuthenticationSchemes = Messenger.Authentication.AuthSchemes.Default)]
public class MessageHub : Hub<IMessageClient>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public MessageHub(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        await Clients.Others.UserJoined(new UserDto2()
        {
            User = this.Context.User!.FindFirst(x => x.Type.EndsWith("name"))!.Value,
            UserId = this.Context.UserIdentifier
        });
    }

    public async Task SendMessage(string text, string? replyToId) 
    {
        _currentUserService.SetCurrentUser(this.Context.User!);

        var message = await _mediator.Send(new PostMessageCommand(null!, text, replyToId));

        await Clients.All.MessageReceived(message);
    }

    public async Task MessageRead(string id)
    {
        _currentUserService.SetCurrentUser(this.Context.User!);

        ReceiptDto receipt = await _mediator.Send(new SendMessageReceiptCommand(id));

        await Clients.All.MessageRead(receipt);
    }

    public async Task EditMessage(string id, string message) 
    {
        _currentUserService.SetCurrentUser(this.Context.User!);

        await _mediator.Send(new UpdateMessageCommand(id, message));
        await Clients.All.MessageEdited(new MessageEditedDto {
            Id = id,
            Text = message,
            Edited = DateTime.Now
        });
    }

    public async Task DeleteMessage(string id) 
    {
        _currentUserService.SetCurrentUser(this.Context.User!);

        await _mediator.Send(new DeleteMessageCommand(id));
        await Clients.All.MessageDeleted(new MessageDeletedDto {
            Id = id
        });
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.Others.UserLeft(new UserDto2()
        {
            User = this.Context.User!.FindFirst(x => x.Type.EndsWith("name"))!.Value,
            UserId = this.Context.UserIdentifier
        });

        await base.OnDisconnectedAsync(exception);
    }
}