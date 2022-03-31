
using Messenger.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Messenger.Contracts;
using MassTransit;

namespace Messenger.Application.Messages.Commands;

public record SendMessageReceiptCommand(string MessageId) : IRequest<ReceiptDto>
{
    public class SendMessageReceiptCommandHandler : IRequestHandler<SendMessageReceiptCommand, ReceiptDto>
    {
        private readonly IMessengerContext context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBus _bus;

        public SendMessageReceiptCommandHandler(IMessengerContext context, ICurrentUserService currentUserService, IBus bus)
        {
            this.context = context;
            _currentUserService = currentUserService;
            _bus = bus;
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

            var dto = receipt.ToDto();

            await _bus.Publish(new MessageRead(dto));

            return dto;
        }
    }
}
