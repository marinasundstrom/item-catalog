using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;
using Catalog.Notifications.Client;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Items.EventHandlers;

public class CommentPostedEventHandler : INotificationHandler<DomainEventNotification<CommentPostedEvent>>
{
    private readonly ICatalogContext context;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationsClient _notificationsClient;
    private readonly ISubscriptionsClient _subscriptionsClient;

    public CommentPostedEventHandler(
        ICatalogContext context,
        ICurrentUserService currentUserService,
        INotificationsClient notificationsClient,
        ISubscriptionsClient subscriptionsClient)
    {
        this.context = context;
        _currentUserService = currentUserService;
        _notificationsClient = notificationsClient;
        _subscriptionsClient = subscriptionsClient;
    }

    public async Task Handle(DomainEventNotification<CommentPostedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var item = await context.Items
            .FirstOrDefaultAsync(i => i.Id == domainEvent.ItemId, cancellationToken);

        if (item is null) return;

        item.CommentCount = await context.Comments
            .AsNoTracking()
            .CountAsync(c => c.Item.Id == domainEvent.ItemId);

        await context.SaveChangesAsync(cancellationToken);

        await SendNotification(item, domainEvent, cancellationToken);
    }

    private async Task SendNotification(Item item, CommentPostedEvent commentPostedEvent, CancellationToken cancellationToken)
    { 
        var comment = await context.Comments
            .Include(c => c.CreatedBy)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(i => i.Id == commentPostedEvent.CommentId, cancellationToken);

        if (comment is null) return;

        await _subscriptionsClient.CreateSubscriptionAsync(new CreateSubscriptionDto()
        {
            UserId = comment.CreatedById,
            SubscriptionGroupId = item.SubscriptionGroupId
        });

        try
        {
            await _notificationsClient.CreateNotificationAsync(new CreateNotificationDto()
            {
                Title = $"{comment.CreatedBy!.GetDisplayName()} commented on {item.Name}.",
                Text = comment.Text,
                Link = $"/items/{item.Id}#comment-{comment.Id}",
                SubscriptionGroupId = item.SubscriptionGroupId,
                ExceptUserIds = new[] { comment.CreatedById }
            });
        }
        catch(Exception exc)
        {
            Console.WriteLine(exc);
        }
    }
}