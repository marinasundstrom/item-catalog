
using Catalog.Application.Common.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Catalog.WebApi.Hubs;

[Authorize]
public class MessageHub : Hub<IMessageClient>
{
    public async Task SendMessage(string message) 
    {
        await Clients.All.MessageReceived(new MessageDto() {
            SentBy = this.Context.User!.FindFirst(x => x.Type.EndsWith("name"))!.Value,
            SentById = this.Context.UserIdentifier,
            Content = message,
            DateSent = DateTime.Now
        });
    }
}