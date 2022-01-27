
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using Catalog.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Notifications.Queries;

public class GetNotificationsQuery : IRequest<Results<NotificationDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; }
    public Application.Common.Models.SortDirection? SortDirection { get; }

    public GetNotificationsQuery(int page, int pageSize, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null)
    {
        Page = page;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Results<NotificationDto>>
    {
        private readonly ICatalogContext context;

        public GetNotificationsQueryHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<Results<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Notifications
                .OrderByDescending(n => n.Published)
                .AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(
                    request.SortBy,
                    request.SortDirection == Application.Common.Models.SortDirection.Desc ? Application.SortDirection.Descending : Application.SortDirection.Ascending);
            }

            query = query.Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable();

            var notifications = await query.ToListAsync(cancellationToken);

            return new Results<NotificationDto>(
                notifications.Select(notification => new NotificationDto(notification.Id, notification.Published, notification.Title, notification.Text, notification.IsRead, notification.Created, notification.CreatedBy, notification.LastModified, notification.LastModifiedBy)),
                totalCount);
        }
    }
}
