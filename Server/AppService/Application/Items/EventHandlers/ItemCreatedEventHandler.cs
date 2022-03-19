using System;

using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using Catalog.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Notifications.Client;

namespace Catalog.Application.Items.EventHandlers;

public class ItemCreatedEventHandler : INotificationHandler<DomainEventNotification<ItemCreatedEvent>>
{
    private readonly ICatalogContext _context;
    private readonly ISubscriptionGroupsClient _subscriptionGroupsClient;
    private readonly IUrlHelper _urlHelper;
    private readonly IItemsClient _itemsClient;

    public ItemCreatedEventHandler(
        ICatalogContext context,
        ISubscriptionGroupsClient subscriptionGroupsClient, 
        IUrlHelper urlHelper,
        IItemsClient itemsClient)
    {
        _context = context;
        _subscriptionGroupsClient = subscriptionGroupsClient;
        _urlHelper = urlHelper;
        _itemsClient = itemsClient;
    }

    public async Task Handle(DomainEventNotification<ItemCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var item = await _context.Items
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .AsSplitQuery()
            .FirstAsync(i => i.Id == domainEvent.ItemId, cancellationToken);

        var subscriptionGroupId = await _subscriptionGroupsClient.CreateSubscriptionGroupAsync(new CreateSubscriptionGroupDto()
        {
            Name = $"item-{item.Id}"
        });

        item.SubscriptionGroupId = subscriptionGroupId;

        await _context.SaveChangesAsync(cancellationToken);

        await PublishNotification(item);
    }

    private async Task PublishNotification(Domain.Entities.Item item)
    {
        var itemDto = new ItemDto(item.Id, item.Name, item.Description, _urlHelper.CreateImageUrl(item.Image), item.CommentCount, item.Created, item.CreatedBy!.ToDto()!, item.LastModified, item.LastModifiedBy?.ToDto());

        await _itemsClient.ItemAdded(itemDto);
    }
}