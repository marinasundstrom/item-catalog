
using MediatR;
using Catalog.Domain.Entities;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Models;

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

            context.Items.Add(item);
            await context.SaveChangesAsync();

            var itemDto = new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image), item.Created, item.CreatedBy, item.LastModified, item.LastModifiedBy);

            await client.ItemAdded(itemDto);

            return Unit.Value;
        }
    }
}
