using Microsoft.AspNetCore.SignalR;

using Catalog.WebApi;
using System.Security.Claims;

public class EmailBasedUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Email)?.Value!;
    }
}