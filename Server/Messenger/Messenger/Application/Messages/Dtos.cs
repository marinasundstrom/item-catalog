using Messenger.Application.Users;

namespace Messenger.Application.Messages;

public record MessageDto(
    string Id, string? Text,
    MessageDto? ReplyTo,
    IEnumerable<ReceiptDto>? Receipts,
    IEnumerable<MessageDto>? Replies,
    DateTime Sent, UserDto SentBy, DateTime? LastModified, UserDto? LastModifiedBy, DateTime? Deleted, UserDto? DeletedBy);

public record ReceiptDto(string Id, string MessageId, UserDto User, DateTime Date);