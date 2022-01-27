
using System.ComponentModel.DataAnnotations;

using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using Catalog.Application.Notifications;
using Catalog.Application.Notifications.Commands;
using Catalog.Application.Notifications.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class NotificationsController : Controller
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator, INotificationClient notificationClient)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<NotificationsResults>> GetNotifications(
        bool includeUnreadNotificationsCount = false,
        int page = 1, int pageSize = 5, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetNotificationsQuery(includeUnreadNotificationsCount, page - 1, pageSize, sortBy, sortDirection), cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult> CreateNotification(CreateNotificationDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateNotificationCommand(dto.Title, dto.Text, dto.Link), cancellationToken);

        return Ok();
    }

    [HttpPost("{id}/MarkAsRead")]
    public async Task<ActionResult> MarkNotificationAsRead(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarkNotificationAsReadCommand(id), cancellationToken);

        return Ok();
    }

    [HttpPost("MarkAllAsRead")]
    public async Task<ActionResult> MarkAllNotificationsAsRead(CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarkAllNotificationsAsReadCommand(), cancellationToken);

        return Ok();
    }

    [HttpGet("UnreadCount")]
    public async Task<ActionResult<int>> GetUnreadNotificationsCount(CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetUnreadNotificationsCountQuery(), cancellationToken));
    }
}

public class CreateNotificationDto
{
    [Required]
    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    public string? Link { get; set; }
}
