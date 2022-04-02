
using Catalog.Messenger.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using MassTransit;
using Catalog.Messenger.Contracts;

namespace Catalog.Messenger.Application.Messages.Commands;

public record DeleteMessageCommand(string MessageId) : IRequest
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand>
    {
        private readonly IMessengerContext context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBus _bus;

        public DeleteMessageCommandHandler(IMessengerContext context, ICurrentUserService currentUserService, IBus bus)
        {
            this.context = context;
            _currentUserService = currentUserService;
            _bus = bus;
        }

        public async Task<Unit> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await context.Messages
                .FirstOrDefaultAsync(i => i.Id == request.MessageId, cancellationToken);

            if (message is null) throw new Exception();

            if (!IsAuthorizedToDelete(message)) 
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            message.Text = string.Empty;
            context.Messages.Remove(message);

            await context.SaveChangesAsync(cancellationToken);

            await _bus.Publish(new MessageDeleted(null!, message.Id));

            return Unit.Value;
        }

        private bool IsAuthorizedToDelete(Domain.Entities.Message message) => _currentUserService.IsCurrentUser(message.CreatedById!) || _currentUserService.IsUserInRole(Roles.Administrator);
    }
}