
using System.Data.Common;

using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Search.Commands;

public class SearchCommand : IRequest<IEnumerable<SearchResultItem>>
{
    public string SearchText { get; set; } = null!;

    public SearchCommand(string searchText)
    {
        SearchText = searchText;
    }

    public class SearchCommandHandler : IRequestHandler<SearchCommand, IEnumerable<SearchResultItem>>
    {
        private readonly ICatalogContext context;

        public SearchCommandHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SearchResultItem>> Handle(SearchCommand request, CancellationToken cancellationToken)
        {
            var searchText = request.SearchText.Trim().ToLower();

            return await context.Items.Where(i =>
                i.Name.Trim().ToLower().Contains(searchText)
                || i.Description.Trim().ToLower().Contains(searchText))
                .Select(i => new SearchResultItem() {
                    Title = i.Name,
                    ResultType = SearchResultItemType.Item,
                    ItemId = i.Id
                }).ToArrayAsync(cancellationToken);
        }
    }
}
