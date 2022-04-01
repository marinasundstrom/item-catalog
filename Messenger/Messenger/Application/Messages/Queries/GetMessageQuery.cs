
using Messenger.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Messenger.Contracts;

namespace Messenger.Application.Messages.Queries;

public class GetMessageQuery : IRequest<MessageDto?>
{
    public string Id { get; set; }

    public GetMessageQuery(string id)
    {
        Id = id;
    }

    public class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, MessageDto?>
    {
        private readonly IMessengerContext context;

        public GetMessageQueryHandler(IMessengerContext context)
        {
            this.context = context;
        }

        public async Task<MessageDto?> Handle(GetMessageQuery request, CancellationToken cancellationToken)
        {
            var message = await context.Messages
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (message is null) return null;

            return message.ToDto();
        }
    }
}