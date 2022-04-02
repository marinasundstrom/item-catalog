
using Catalog.IdentityService.Application.Common.Interfaces;
using Catalog.IdentityService.Contracts;

using MassTransit;

using MediatR;

namespace Catalog.IdentityService.Consumers;

public class GetUserConsumer : IConsumer<GetUser>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public GetUserConsumer(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task Consume(ConsumeContext<GetUser> context)
    {
        var message = context.Message;

        var user = await _mediator.Send(new Catalog.IdentityService.Application.Users.Queries.GetUserQuery(message.UserId));

        await context.RespondAsync(new GetUserResponse(user.Id, user.FirstName, user.LastName, user.DisplayName, user.Email));
    }
}