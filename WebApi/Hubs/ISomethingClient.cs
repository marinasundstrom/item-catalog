namespace WebApi.Hubs;

public interface ISomethingClient
{
    Task ResponseReceived(string message);
}