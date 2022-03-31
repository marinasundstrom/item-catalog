
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Common.Models;
using Messenger.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Messenger.Contracts;

namespace Messenger.Application.Conversations.Queries;

public class GetConversationsQuery : IRequest<Results<ConversationDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; }
    public Application.Common.Models.SortDirection? SortDirection { get; }

    public GetConversationsQuery(int page, int pageSize, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null)
    {
        Page = page;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public class GetConversationsQueryHandler : IRequestHandler<GetConversationsQuery, Results<ConversationDto>>
    {
        private readonly IMessengerContext context;

        public GetConversationsQueryHandler(IMessengerContext context)
        {
            this.context = context;
        }

        public async Task<Results<ConversationDto>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Conversations
                .Include(c => c.CreatedBy)
                .Include(c => c.LastModifiedBy)
                .OrderByDescending(c => c.Created)
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

            query = query.Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable();

            var conversations = await query.ToListAsync(cancellationToken);

            return new Results<ConversationDto>(
                conversations.Select(message => message.ToDto()),
                totalCount);
        }
    }
}