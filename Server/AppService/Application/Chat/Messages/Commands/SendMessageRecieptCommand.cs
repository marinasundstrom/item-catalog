
using Catalog.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Messages.Commands;

public record SendMessageRecieptCommand(string MessageId) : IRequest
{
    public class SendMessageRecieptCommandHandler : IRequestHandler<SendMessageRecieptCommand>
    {
        private readonly ICatalogContext context;

        public SendMessageRecieptCommandHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(SendMessageRecieptCommand request, CancellationToken cancellationToken)
        {
            var message = await context
                .Messages
                .Include(x => x.Receipts)
                .AsSplitQuery()
                .FirstOrDefaultAsync(i => i.Id == request.MessageId, cancellationToken);

            if (message is null) throw new Exception();

            message.Receipts.Add(new Domain.Entities.MessageReceipt()
            {
                Id = Guid.NewGuid().ToString()
            });

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
