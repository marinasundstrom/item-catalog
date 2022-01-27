
using Catalog.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Notifications.Queries;

public class GetUnreadNotificationsCountQuery : IRequest<int>
{
    public class GetUnreadNotificationsCountQueryHandler : IRequestHandler<GetUnreadNotificationsCountQuery, int>
    {
        private readonly ICatalogContext context;

        public GetUnreadNotificationsCountQueryHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<int> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            var unreadNotificationsCount = await context.Notifications
                .OrderByDescending(n => n.Published)
                .Where(n => !n.IsRead)
                .CountAsync(cancellationToken);

            return unreadNotificationsCount;
        }
    }
}