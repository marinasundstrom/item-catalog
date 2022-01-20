﻿
using Catalog.Application;

using Microsoft.AspNetCore.SignalR;

using Catalog.Application.Common.Interfaces;

using Catalog.WebApi.Hubs;
using Catalog.Application.Models;

namespace Catalog.WebApi.Services;

public class ItemsClient : IItemsClient
{
    private readonly IHubContext<ItemsHub, IItemsClient> _itemsHubContext;

    public ItemsClient(IHubContext<ItemsHub, IItemsClient> itemsHubContext)
    {
        _itemsHubContext = itemsHubContext;
    }

    public async Task ImageUploaded(string id, string image)
    {
        await _itemsHubContext.Clients.All.ImageUploaded(id, image);
    }

    public async Task ItemAdded(ItemDto item)
    {
        await _itemsHubContext.Clients.All.ItemAdded(item);
    }

    public async Task ItemDeleted(string id, string name)
    {
        await _itemsHubContext.Clients.All.ItemDeleted(id, name);
    }
}

