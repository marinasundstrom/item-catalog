
using MediatR;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;

namespace Catalog.Application.Queries;

public class GetItemQuery : IRequest<ItemDto?>
{
    public string Id { get; set; }

    public GetItemQuery(string id)
    {
        Id = id;
    }

    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ItemDto?>
    {
        private readonly IUnitOfWork context;
        private readonly IUrlHelper urlHelper;

        public GetItemQueryHandler(IUnitOfWork context, IUrlHelper urlHelper)
        {
            this.context = context;
            this.urlHelper = urlHelper;
        }

        public async Task<ItemDto?> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var item = await context.Items.GetAsync(request.Id, cancellationToken);

            if (item == null) return null;

            return new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image!), item.Created, item.CreatedBy, item.LastModified, item.LastModifiedBy);
        }
    }
}
