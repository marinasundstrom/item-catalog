
using Catalog.IdentityService.Contracts;
using Catalog.Search.Application.Common.Interfaces;
using Catalog.Search.Application.Users.Commands;

using MassTransit;

using MediatR;

namespace Catalog.Search.Consumers;

public class UserDeleted5Consumer : IConsumer<UserDeleted>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public UserDeleted5Consumer(IMediator mediator, ICurrentUserService currentUserService)
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