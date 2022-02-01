
using Worker.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Hangfire;

namespace Worker.Application.Notifications.Commands;

public class DeleteNotificationCommand : IRequest
{
    public DeleteNotificationCommand(string notificationId)
    {
        NotificationId = notificationId;
    }

    public string NotificationId { get; }

    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
    {
        private readonly IWorkerContext context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public DeleteNotificationCommandHandler(IWorkerContext context, ICurrentUserService currentUserService, IBackgroundJobClient backgroundJobClient)
        {
            this.context = context;
            _currentUserService = currentUserService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await context.Notifications
                .Where(n => n.UserId == _currentUserService.UserId || n.UserId == null)
                .FirstOrDefaultAsync(i => i.Id == request.NotificationId, cancellationToken);

            if (notification is null)
            {
                throw new Exception();
            }

            if(notification.ScheduledJobId is not null && notification.Published is null)
            {
                _backgroundJobClient.Delete(notification.ScheduledJobId);
            }

            context.Notifications.Remove(notification);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
