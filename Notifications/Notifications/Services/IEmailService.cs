
namespace Notifications.Services;

public interface IEmailService
{
    Task Send(string emailAddress, string subject, string content);
}