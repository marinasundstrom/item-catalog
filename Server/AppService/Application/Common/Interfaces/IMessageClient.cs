namespace Catalog.Application.Common.Interfaces;

public interface IMessageClient
{
    Task MessageReceived(MessageDto message);
}

public class MessageDto
{
    public string SentBy { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime DateSent { get; set; }
    public string? SentById { get; set; }
}
