
using Messenger.Application.Common.Models;
using Messenger.Application.Messages;
using Messenger.Application.Messages.Queries;
using Messenger.Application.Messages.Commands;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Messenger.Contracts;

namespace Messenger.WebApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = Messenger.Authentication.AuthSchemes.Default)]
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(
        string id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteMessageCommand(id), cancellationToken);
        return Ok();
    }
}