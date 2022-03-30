
using Messenger.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Application.Messages.Commands;

public record SendMessageReceiptCommand(string MessageId) : IRequest<ReceiptDto>
{
    public class SendMessageReceiptCommandHandler : IRequestHandler<SendMessageReceiptCommand, ReceiptDto>
    {
        private readonly IMessengerContext context;
        private readonly ICurrentUserService _currentUserService;

        public SendMessageReceiptCommandHandler(IMessengerContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<ReceiptDto> Handle(SendMessageReceiptCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var message = await context
                .Messages
                .Include(x => x.Receipts.Where(i => i.CreatedById != userId))
                .IgnoreQueryFilters()
                .AsSplitQuery()
                .FirstOrDefaultAsync(i => i.Id == request.MessageId, cancellationToken);

            if (message is null) throw new Exception();

            var receipt = new Domain.Entities.MessageReceipt()
            {
                Id = Guid.NewGuid().ToString()
            };

            message.Receipts.Add(receipt);

            await context.SaveChangesAsync(cancellationToken);

            receipt = await context
                .MessageReceipts
                .Include(x => x.CreatedBy)
                .IgnoreQueryFilters()
                .AsSplitQuery()
                .FirstAsync(i => i.Id == receipt.Id, cancellationToken);

            return receipt.ToDto();
        }
    }
}
