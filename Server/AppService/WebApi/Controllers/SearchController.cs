
using Catalog.Application.Search;
using Catalog.Application.Search.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class SearchController : Controller
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<SearchResultItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SearchResultItem>>> Search(string searchText, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new SearchCommand(searchText), cancellationToken));
    }
}
