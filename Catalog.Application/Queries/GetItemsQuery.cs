
using MediatR;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;
using Catalog.Domain;

namespace Catalog.Application.Queries;

public class GetItemsQuery : IRequest<Results<ItemDto>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }
    public string? SortBy { get; }
    public SortDirection? SortDirection { get; }

    public GetItemsQuery(int page, int pageSize, string? sortBy = null, SortDirection? sortDirection = null)
    {
        Page = page;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, Results<ItemDto>>
    {
        private readonly IUnitOfWork context;
        private readonly IUrlHelper urlHelper;

        public GetItemsQueryHandler(IUnitOfWork context, IUrlHelper urlHelper)
        {
            this.context = context;
            this.urlHelper = urlHelper;
        }

        public async Task<Results<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var specification = new PagedItemsSpecification(request.Page, request.PageSize);

            if(request.SortBy is not null) 
            {
                if(request.SortDirection == Application.SortDirection.Asc) 
                {
                    specification.AddOrderBy(request.SortBy);
                }
                else if(request.SortDirection == Application.SortDirection.Desc) 
                {
                    specification.AddOrderByDescending(request.SortBy);
                }
            }

            var items = await context.Items.GetAllAsync(specification, cancellationToken);

            var totalCount = await context.Items.CountAsync(
                new ItemsOrderByCreatedSpecification(), cancellationToken);

            return new Results<ItemDto>(
                items.Select(item => new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image), item.Created, item.CreatedBy, item.LastModified, item.LastModifiedBy)),
                totalCount);
        }
    }
}
