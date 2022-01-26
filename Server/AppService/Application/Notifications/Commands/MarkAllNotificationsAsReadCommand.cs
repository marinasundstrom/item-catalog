
using Catalog.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Notifications.Commands;

public class MarkAllNotificationsAsReadCommand : IRequest
{
    public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand>
    {
        private readonly ICatalogContext context;

        public MarkAllNotificationsAsReadCommandHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var notifications = await context.Notifications.ToListAsync(cancellationToken);

            foreach(var notification in notifications)
            {
                notification.IsRead = true;
            }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}