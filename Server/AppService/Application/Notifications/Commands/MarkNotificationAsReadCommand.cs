
using System.Data.Common;

using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Notifications.Commands;

public class MarkNotificationAsReadCommand : IRequest
{
    public MarkNotificationAsReadCommand(string notificationId)
    {
        NotificationId = notificationId;
    }

    public string NotificationId { get; }

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly ICatalogContext context;
        private readonly ICurrentUserService _currentUserService;

        public MarkNotificationAsReadCommandHandler(ICatalogContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await context.Notifications
                .Where(n => n.UserId == _currentUserService.UserId)
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
