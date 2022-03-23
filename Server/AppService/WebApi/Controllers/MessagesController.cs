
using Catalog.Application.Common.Models;
using Catalog.Application.Messages;
using Catalog.Application.Messages.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<Results<MessageDto>>> GetMessages(
        int skip = 0, int take = 10, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetMessagesIncrQuery(null!, skip, take, sortBy, sortDirection), cancellationToken));
    }
}