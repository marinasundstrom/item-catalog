namespace WebApi.Hubs;

public interface INotificationClient
{
    Task NotificationReceived(string message);
}