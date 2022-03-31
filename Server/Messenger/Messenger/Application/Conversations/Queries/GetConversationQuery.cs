
using Messenger.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Messenger.Contracts;

namespace Messenger.Application.Conversations.Queries;

public class GetConversationQuery : IRequest<ConversationDto?>
{
    public string Id { get; set; }

    public GetConversationQuery(string id)
    {
        Id = id;
    }

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