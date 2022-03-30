
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Common.Models;
using Messenger.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Messenger.Contracts;

namespace Messenger.Application.Messages.Queries;

public class GetMessagesQuery : IRequest<Results<MessageDto>>
{
    public string ItemId { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; }
    public Application.Common.Models.SortDirection? SortDirection { get; }

    public GetMessagesQuery(string itemId, int page, int pageSize, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null)
    {
        ItemId = itemId;
        Page = page;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, Results<MessageDto>>
    {
        private readonly IMessengerContext context;

        public GetMessagesQueryHandler(IMessengerContext context)
        {
            this.context = context;
        }

        public async Task<Results<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var query = context.Messages
                .Include(c => c.CreatedBy)
                .Include(c => c.LastModifiedBy)
                //.Where(c => c.Item.Id == request.ItemId)
                .OrderByDescending(c => c.Created)
                .AsSplitQuery()
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

            var messages = await query.ToListAsync(cancellationToken);

            return new Results<MessageDto>(
                messages.Select(message => message.ToDto()),
                totalCount);
        }
    }
}