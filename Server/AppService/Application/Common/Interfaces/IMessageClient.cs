namespace Catalog.Application.Common.Interfaces;

public interface IMessageClient
{
    Task UserJoined(UserDto2 user);

    Task MessageReceived(MessageDto message);

    Task MessageDeleted(MessageDeletedDto dto);

    Task MessageEdited(MessageEditedDto dto);

    Task UserLeft(UserDto2 user);
}

public class MessageDeletedDto
{
    public string? Id { get; set; }
}


public class MessageEditedDto
{
    public string? Id { get; set; }
}

public class MessageDto
{
    public string? Id { get; set; }
    
    public string SentBy { get; set; } = null!;

    public string SentById { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime DateSent { get; set; }
}

public class UserDto2
{
    public string User { get; set; } = null!;
    public string? UserId { get; set; }
}
