
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Messages.Commands;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Catalog.WebApi.Hubs;

[Authorize]
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

    public async Task SendMessage(string message) 
    {
        _currentUserService.SetCurrentUser(this.Context.UserIdentifier!);

        var id = await _mediator.Send(new PostMessageCommand(null!, message));

        await Clients.All.MessageReceived(new MessageDto() {
            Id = id,
            SentBy = this.Context.User!.FindFirst(x => x.Type.EndsWith("name"))!.Value,
            SentById = this.Context.UserIdentifier,
            Content = message,
            DateSent = DateTime.Now
        });
    }

    public async Task EditMessage(string id, string message) 
    {
        _currentUserService.SetCurrentUser(this.Context.UserIdentifier!);

        await _mediator.Send(new UpdateMessageCommand(id, message));
        await Clients.All.MessageEdited(new MessageEditedDto {
            Id = id
        });
    }

    public async Task DeleteMessage(string id) 
    {
        _currentUserService.SetCurrentUser(this.Context.UserIdentifier!);

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