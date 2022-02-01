
using System.Data.Common;

using Worker.Application.Common.Interfaces;
using Worker.Domain.Entities;
using Worker.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Worker.Application.Notifications.Commands;

public class MarkNotificationAsReadCommand : IRequest
{
    public MarkNotificationAsReadCommand(string notificationId)
    {
        NotificationId = notificationId;
    }

    public string NotificationId { get; }

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly IWorkerContext context;
        private readonly ICurrentUserService _currentUserService;

        public MarkNotificationAsReadCommandHandler(IWorkerContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await context.Notifications
                .Where(n => n.UserId == _currentUserService.UserId || n.UserId == null)
                .FirstOrDefaultAsync(i => i.Id == request.NotificationId, cancellationToken);

            if (notification is null)
            {
                throw new Exception();
            }

            notification.IsRead = true;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
