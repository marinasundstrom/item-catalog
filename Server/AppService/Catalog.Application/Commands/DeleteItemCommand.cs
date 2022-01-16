
using MediatR;
using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure.Repositories;

namespace Catalog.Application.Commands;

public class DeleteItemCommand : IRequest<DeletionResult>
{
    public string Id { get; set; }

    public DeleteItemCommand(string id)
    {
        Id = id;
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, DeletionResult>
    {
        private readonly IUnitOfWork context;
        private readonly IItemsClient client;

        public DeleteItemCommandHandler(IUnitOfWork context, IItemsClient client)
        {
            this.context = context;
            this.client = client;
        }

        public async Task<DeletionResult> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.GetAsync(request.Id, cancellationToken);

            if (item == null)
            {
                return DeletionResult.NotFound;
            }

            context.Items.Remove(item);

            await context.SaveChangesAsync();

            await client.ItemDeleted(item.Id, item.Name);

            return DeletionResult.Successful;
        }
    }
}