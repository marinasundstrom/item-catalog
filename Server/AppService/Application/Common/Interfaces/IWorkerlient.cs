namespace Catalog.Application.Common.Interfaces;

public interface IWorkerlient
{
    Task NotificationReceived(string message);
}