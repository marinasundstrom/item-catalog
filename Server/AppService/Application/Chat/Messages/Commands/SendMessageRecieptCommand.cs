
using Catalog.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Messages.Commands;

public record SendMessageReceiptCommand(string MessageId) : IRequest
{
    public class SendMessageReceiptCommandHandler : IRequestHandler<SendMessageReceiptCommand>
    {
        private readonly ICatalogContext context;
        private readonly ICurrentUserService _currentUserService;

        public SendMessageReceiptCommandHandler(ICatalogContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(SendMessageReceiptCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var message = await context
                .Messages
                .Include(x => x.Receipts.Where(i => i.CreatedById != userId))
                .IgnoreQueryFilters()
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
