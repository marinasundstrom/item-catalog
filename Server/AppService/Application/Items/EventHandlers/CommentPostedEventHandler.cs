using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Worker.Client;

namespace Catalog.Application.Items.EventHandlers;

public class CommentPostedEventHandler : INotificationHandler<DomainEventNotification<CommentPostedEvent>>
{
    private readonly ICatalogContext context;
    private readonly INotificationsClient _notificationsClient;

    public CommentPostedEventHandler(ICatalogContext context, INotificationsClient notificationsClient)
    {
        this.context = context;
        _notificationsClient = notificationsClient;
    }

    public async Task Handle(DomainEventNotification<CommentPostedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var item = await context.Items
            .FirstOrDefaultAsync(i => i.Id == domainEvent.ItemId, cancellationToken);

        if (item is null) return;

        item.CommentCount = await context.Comments.CountAsync(c => c.Item.Id == domainEvent.ItemId);

        await context.SaveChangesAsync(cancellationToken);

        await SendNotification(item, domainEvent, cancellationToken);
    }

    private async Task SendNotification(Item item, CommentPostedEvent commentPostedEvent, CancellationToken cancellationToken)
    { 
        var comment = await context.Comments
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(i => i.Id == commentPostedEvent.CommentId, cancellationToken);

        if (comment is null) return;

        try
        {
            await _notificationsClient.CreateNotificationAsync(new Worker.Client.CreateNotificationDto()
            {
                Title = $"{comment.CreatedBy!.FirstName} commented on {item.Name}.",
                Text = comment.Text,
                UserId = item.CreatedBy.Id,
                Link = $"/items/{item.Id}#comment-{comment.Id}"
            });
        }
        catch(Exception exc)
        {
            Console.WriteLine(exc);
        }
    }
}