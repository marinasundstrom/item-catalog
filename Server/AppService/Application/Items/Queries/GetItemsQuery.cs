
using MediatR;
using Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Models;

namespace Catalog.Application.Items.Queries;

public class GetItemsQuery : IRequest<Results<ItemDto>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }
    public string? SortBy { get; }
    public Models.SortDirection? SortDirection { get; }

    public GetItemsQuery(int page, int pageSize, string? sortBy = null, Models.SortDirection? sortDirection = null)
    {
        Page = page;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, Results<ItemDto>>
    {
        private readonly ICatalogContext context;
        private readonly IUrlHelper urlHelper;

        public GetItemsQueryHandler(ICatalogContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            this.urlHelper = urlHelper;
        }

        public async Task<Results<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Items
                .OrderBy(i => i.Created)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable();


            var totalCount = await query.CountAsync();

            if (request.SortBy is not null)
            {
                query = query.OrderBy(
                    request.SortBy,
                    request.SortDirection == Models.SortDirection.Desc ? Application.SortDirection.Descending : Application.SortDirection.Ascending);
            }

            var items = await query.ToListAsync(cancellationToken);

            return new Results<ItemDto>(
                items.Select(item => new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image), item.Created, item.CreatedBy, item.LastModified, item.LastModifiedBy)),
                totalCount);
        }
    }
}
