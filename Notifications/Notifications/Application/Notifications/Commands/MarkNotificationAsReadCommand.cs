
using System.Data.Common;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Notifications.Application.Common.Interfaces;
using Notifications.Domain.Entities;
using Notifications.Domain.Events;

namespace Notifications.Application.Notifications.Commands;

public record MarkNotificationAsReadCommand(string NotificationId) : IRequest
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly INotificationsContext context;

        public MarkNotificationAsReadCommandHandler(INotificationsContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await context.Notifications
                .Where(n => n.Published != null)
                .FirstOrDefaultAsync(i => i.Id == request.NotificationId, cancellationToken);

            if (notification is null)
            {
                throw new Exception();
            }

            notification.IsRead = true;
            notification.Read = DateTime.Now;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}