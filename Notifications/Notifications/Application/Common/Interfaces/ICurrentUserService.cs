namespace Catalog.Notifications.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }

    void SetCurrentUser(string userId);
}