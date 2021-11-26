using System;

using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public class ItemsHub : Hub<IItemsClient>
{

}