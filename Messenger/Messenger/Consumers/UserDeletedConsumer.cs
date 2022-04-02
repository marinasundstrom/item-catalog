
using MassTransit;

using MediatR;

using Catalog.IdentityService.Contracts;
using Catalog.Messenger.Application.Common.Interfaces;
using Catalog.Messenger.Application.Users.Commands;

namespace Catalog.Messenger.Consumers;

public class UserDeleted2Consumer : IConsumer<UserDeleted>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public UserDeleted2Consumer(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task Consume(ConsumeContext<UserDeleted> context)
    {
        var message = context.Message;

        _currentUserService.SetCurrentUser(message.DeletedById);

        var result = await _mediator.Send(new DeleteUserCommand(message.UserId));
    }
}