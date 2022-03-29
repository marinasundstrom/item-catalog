using Catalog.Application.Messages;

namespace Catalog.Application.Common.Interfaces;

public interface IMessageClient
{
    Task UserJoined(UserDto2 user);

    Task MessageReceived(MessageDto message);

    Task MessageRead(ReceiptDto receipt);

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

public class UserDto2
{
    public string User { get; set; } = null!;
    public string? UserId { get; set; }
}
