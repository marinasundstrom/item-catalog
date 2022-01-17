
using Microsoft.AspNetCore.SignalR;

using Catalog.Application.Common.Interfaces;

namespace Catalog.WebApi.Hubs;

public class SomethingHub : Hub<ISomethingClient>
{

}
