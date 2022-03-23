
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

        await Clients.All.UserJoined(new UserDto2()
        {
            User = this.Context.User!.FindFirst(x => x.Type.EndsWith("name"))!.Value,
            UserId = this.Context.UserIdentifier
        });
    }

    public async Task SendMessage(string message) 
    {
        _currentUserService.SetCurrentUser(this.Context.UserIdentifier!);

        await _mediator.Send(new PostMessageCommand(null!, message));

        await Clients.All.MessageReceived(new MessageDto() {
            SentBy = this.Context.User!.FindFirst(x => x.Type.EndsWith("name"))!.Value,
            SentById = this.Context.UserIdentifier,
            Content = message,
            DateSent = DateTime.Now
        });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.UserLeft(new UserDto2()
        {
            User = this.Context.User!.FindFirst(x => x.Type.EndsWith("name"))!.Value,
            UserId = this.Context.UserIdentifier
        });

        await base.OnDisconnectedAsync(exception);
    }
}