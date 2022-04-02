
using MediatR;

using Microsoft.EntityFrameworkCore;

using Notifications.Application.Common.Interfaces;

namespace Notifications.Application.Notifications.Queries;

public record GetUnreadNotificationsCountQuery(string? UserId) : IRequest<int>
{
    public class GetUnreadNotificationsCountQueryHandler : IRequestHandler<GetUnreadNotificationsCountQuery, int>
    {
        private readonly INotificationsContext context;

        public GetUnreadNotificationsCountQueryHandler(INotificationsContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
        }

        public async Task<int> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            var query = context.Notifications
                .AsNoTracking()
                .Where(n => n.Published != null)
                .AsQueryable();

            if (request.UserId is not null)
            {
                query = query.Where(n => n.UserId == request.UserId);
            }

            var unreadNotificationsCount = await query
                .Where(n => !n.IsRead)
                .CountAsync(cancellationToken);

            return unreadNotificationsCount;
        }
    }
}