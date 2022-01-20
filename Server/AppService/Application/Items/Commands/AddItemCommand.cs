
using MediatR;
using Catalog.Domain.Entities;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Models;
using Catalog.Domain.Events;

namespace Catalog.Application.Items.Commands;

public class AddItemCommand : IRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public AddItemCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public class AddItemCommandHandler : IRequestHandler<AddItemCommand>
    {
        private readonly ICatalogContext context;
        private readonly IUrlHelper urlHelper;
        private readonly IItemsClient client;

        public AddItemCommandHandler(ICatalogContext context, IUrlHelper urlHelper, IItemsClient client)
        {
            this.context = context;
            this.urlHelper = urlHelper;
            this.client = client;
        }

        public async Task<Unit> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item(Guid.NewGuid().ToString(), request.Name, request.Description);

            item.DomainEvents.Add(new ItemCreatedEvent(item.Id));

            context.Items.Add(item);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
