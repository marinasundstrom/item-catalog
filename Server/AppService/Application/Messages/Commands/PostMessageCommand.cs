
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Notifications.Client;

namespace Catalog.Application.Messages.Commands;

public record PostMessageCommand(string ItemId, string Text) : IRequest<string>
{
    public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand, string>
    {
        private readonly ICatalogContext context;

        public PostMessageCommandHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<string> Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            //var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);

            //if (item is null) throw new Exception();

            var message = new Message(request.Text);

            context.Messages.Add(message);

            await context.SaveChangesAsync(cancellationToken);

            return message.Id;
        }
    }
}
