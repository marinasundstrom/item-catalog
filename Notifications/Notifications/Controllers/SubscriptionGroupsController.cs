
using Catalog.Notifications.Application.Subscriptions.Commands;
using Catalog.Notifications.Application.Subscriptions.Queries;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Notifications.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = Notifications.Authentication.AuthSchemes.Default)]
public class SubscriptionGroupsController : Controller
{
    private readonly IMediator _mediator;

    public SubscriptionGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateSubscriptionGroup(CreateSubscriptionGroupDto dto, CancellationToken cancellationToken)
    {
        string subscriptionGroupId = await _mediator.Send(new CreateSubscriptionGroupCommand(dto.Name), cancellationToken);

        return Ok(subscriptionGroupId);
    }

    [HttpGet("{subscriptionGroupId}")]
    public async Task<ActionResult<SubscriptionGroupDto>> GetSubscriptionGroup(string subscriptionGroupId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSubscriptionGroupQuery(subscriptionGroupId, null), cancellationToken);

        if(result is null) 
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSubscriptionGroup(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteSubscriptionGroupCommand(id), cancellationToken);

        return Ok();
    }

    [HttpPost("{subscriptionGroupId}/Subscriptions")]
    public async Task<ActionResult> AddSubscriptionToGroup(string subscriptionGroupId, AddSubscriptionToGroupDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddSubscriptionToGroupCommand(dto.SubscriptionId, subscriptionGroupId), cancellationToken);

        return Ok();
    }

    [HttpDelete("{subscriptionGroupId}/Subscriptions/{subcriptionId}")]
    public async Task<ActionResult> RemoveSubscriptionToGroup(string subscriptionGroupId, string subcriptionId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveSubscriptionFromGroupCommand(subcriptionId, subscriptionGroupId), cancellationToken);

        return Ok();
    }
}

public class CreateSubscriptionGroupDto
{
    public string? Name { get; set; }
}

public class AddSubscriptionToGroupDto 
{
    public string SubscriptionId { get; set; } = null!;

}