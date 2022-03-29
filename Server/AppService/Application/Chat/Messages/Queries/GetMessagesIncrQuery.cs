
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using Catalog.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Messages.Queries;

public class GetMessagesIncrQuery : IRequest<Results<MessageDto>>
{
    public string ItemId { get; set; }

    public int Skip { get; set; }
    public int Take { get; set; }
    public string? SortBy { get; }
    public Application.Common.Models.SortDirection? SortDirection { get; }

    public GetMessagesIncrQuery(string itemId, int skip, int take, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null)
    {
        ItemId = itemId;
        Skip = skip;
        Take = take;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public class GetMessagesIncrQueryHandler : IRequestHandler<GetMessagesIncrQuery, Results<MessageDto>>
    {
        private readonly ICatalogContext context;

        public GetMessagesIncrQueryHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<Results<MessageDto>> Handle(GetMessagesIncrQuery request, CancellationToken cancellationToken)
        {
            var query = context.Messages
                .Include(c => c.CreatedBy)
                .Include(c => c.LastModifiedBy)
                .Include(c => c.DeletedBy)
                .Include(c => c.Receipts)
                .ThenInclude(r => r.CreatedBy)
                //.Where(c => c.Item.Id == request.ItemId)
                .OrderByDescending(c => c.Created)
                .IgnoreQueryFilters()
                .AsSplitQuery()
                .AsNoTracking()
                .AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(
                    request.SortBy,
                    request.SortDirection == Application.Common.Models.SortDirection.Desc ? Application.SortDirection.Descending : Application.SortDirection.Ascending);
            }

            query = query
                .Skip(request.Skip)
                .Take(request.Take).AsQueryable();

            var messages = await query
                .ToListAsync(cancellationToken);

            return new Results<MessageDto>(
                messages.Select(message => message.ToDto()),
                totalCount);
        }
    }
}