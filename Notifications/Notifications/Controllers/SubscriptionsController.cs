
using System.ComponentModel.DataAnnotations;

using Catalog.Notifications.Application.Subscriptions.Commands;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Notifications.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = Notifications.Authentication.AuthSchemes.Default)]
public class SubscriptionsController : Controller
{
    private readonly IMediator _mediator;

    public SubscriptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubscription(CreateSubscriptionDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateSubscriptionCommand(dto.UserId, dto.SubscriptionGroupId, dto.Tag), cancellationToken);

        return Ok();
    }
}

public class CreateSubscriptionDto
{
    [Required]
    public string UserId { get; set; } = null!;

    public string? SubscriptionGroupId { get; set; }

    public string? Tag { get; set; }
}