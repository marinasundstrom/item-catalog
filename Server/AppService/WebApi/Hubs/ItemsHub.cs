using System;

using Catalog.Application.Common.Interfaces;

using Microsoft.AspNetCore.SignalR;

namespace Catalog.WebApi.Hubs;

public class ItemsHub : Hub<IItemsClient>
{

}
