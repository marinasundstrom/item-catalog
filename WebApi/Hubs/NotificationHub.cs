﻿
using Microsoft.AspNetCore.SignalR;

using Catalog.Application.Common.Interfaces;

namespace WebApi.Hubs;

public class NotificationHub : Hub<INotificationClient>
{

}