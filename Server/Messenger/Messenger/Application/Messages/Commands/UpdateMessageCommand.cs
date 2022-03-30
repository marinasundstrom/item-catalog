
using Messenger.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Application.Messages.Commands;

public record UpdateMessageCommand(string MessageId, string Text) : IRequest
{
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand>
    {
        private readonly IMessengerContext context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateMessageCommandHandler(IMessengerContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await context.Messages.FirstOrDefaultAsync(i => i.Id == request.MessageId, cancellationToken);

            if (message is null) throw new Exception();

            if (!IsAuthorizedToEdit(message))
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            message.Text = request.Text;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private bool IsAuthorizedToEdit(Domain.Entities.Message message) => _currentUserService.IsCurrentUser(message.CreatedById!) || _currentUserService.IsUserInRole(Roles.Administrator);
    }
}
