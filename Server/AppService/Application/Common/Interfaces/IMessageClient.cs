namespace Catalog.Application.Common.Interfaces;

public interface IMessageClient
{
    Task UserJoined(UserDto2 user);

    Task MessageReceived(MessageDto message);

    Task UserLeft(UserDto2 user);
}

public class MessageDto
{
    public string SentBy { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime DateSent { get; set; }
    public string? SentById { get; set; }
}

public class UserDto2
{
    public string User { get; set; } = null!;
    public string? UserId { get; set; }
}
