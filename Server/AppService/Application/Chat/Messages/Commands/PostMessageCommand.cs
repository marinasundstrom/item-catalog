
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Notifications.Client;

namespace Catalog.Application.Messages.Commands;

public record PostMessageCommand(string ItemId, string Text, string ReplyToId) : IRequest<MessageDto>
{
    public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand, MessageDto>
    {
        private readonly ICatalogContext context;

        public PostMessageCommandHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<MessageDto> Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            //var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);

            //if (item is null) throw new Exception();

            var message = new Message(request.Text, request.ReplyToId);

            context.Messages.Add(message);

            await context.SaveChangesAsync(cancellationToken);

            message = await context.Messages
                .Include(c => c.CreatedBy)
                .Include(c => c.LastModifiedBy)
                .Include(c => c.DeletedBy)
                .Include(c => c.Receipts)
                .ThenInclude(r => r.CreatedBy)
                //.Where(c => c.Item.Id == request.ItemId)
                .OrderByDescending(c => c.Created)
                .IgnoreQueryFilters()
                .AsSplitQuery()
                .FirstAsync(x => x.Id == message.Id);

            return message.ToDto();
        }
    }
}
