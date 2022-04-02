
using Catalog.Messenger.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Catalog.Messenger.Contracts;

namespace Catalog.Messenger.Application.Conversations.Queries;

public record GetConversationQuery(string Id) : IRequest<ConversationDto?>
{
    public class GetConversationQueryHandler : IRequestHandler<GetConversationQuery, ConversationDto?>
    {
        private readonly IMessengerContext context;

        public GetConversationQueryHandler(IMessengerContext context)
        {
            this.context = context;
        }

        public async Task<ConversationDto?> Handle(GetConversationQuery request, CancellationToken cancellationToken)
        {
            var conversation = await context.Conversations
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (conversation is null) return null;

            return conversation.ToDto();
        }
    }
}