using MassTransit;

using MediatR;
using Catalog.IdentityService.Contracts;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Users.Commands;
namespace Catalog.Consumers;

public class UserCreated0Consumer : IConsumer<UserCreated>
{
    private readonly IMediator _mediator;
    private readonly IRequestClient<GetUser> _requestClient;
    private readonly ILogger<UserCreated0Consumer> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UserCreated0Consumer(IMediator mediator, ICurrentUserService currentUserService, IRequestClient<GetUser> requestClient, ILogger<UserCreated0Consumer> logger)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _requestClient = requestClient;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        try 
        {
            var message = context.Message;

            _currentUserService.SetCurrentUser(message.CreatedById);

            var messageR = await _requestClient.GetResponse<GetUserResponse>(new GetUser(message.UserId, (message.CreatedById)));
            var message2 = messageR.Message;

            var result = await _mediator.Send(new CreateUserCommand(message2.UserId, message2.FirstName, message2.LastName, message2.DisplayName, message2.Email));
        }
        catch(Exception e) 
        {
        _logger.LogError(e, "FOO"); 
        }
    }
}