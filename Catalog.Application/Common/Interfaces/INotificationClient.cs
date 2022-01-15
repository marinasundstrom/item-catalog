namespace Catalog.Application.Common.Interfaces;

public interface INotificationClient
{
    Task NotificationReceived(string message);
}