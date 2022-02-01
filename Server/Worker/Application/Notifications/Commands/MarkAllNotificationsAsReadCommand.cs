
using Worker.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Worker.Application.Notifications.Commands;

public class MarkAllNotificationsAsReadCommand : IRequest
{
    public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand>
    {
        private readonly IWorkerContext context;
        private readonly ICurrentUserService _currentUserService;

        public MarkAllNotificationsAsReadCommandHandler(IWorkerContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var notifications = await context.Notifications
                .Where(n => n.UserId == _currentUserService.UserId || n.UserId == null)
                .ToListAsync(cancellationToken);

            foreach(var notification in notifications)
            {
                notification.IsRead = true;
            }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}