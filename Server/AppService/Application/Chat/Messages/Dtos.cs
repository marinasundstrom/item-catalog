using Catalog.Application.Users;

namespace Catalog.Application.Messages;

public record MessageDto(
    string Id, string? Text,
    string? ReplyToId,
    IEnumerable<ReceiptDto> Receipts,
    IEnumerable<MessageDto>? Replies,
    DateTime Sent, UserDto SentBy, DateTime? LastModified, UserDto? LastModifiedBy, DateTime? Deleted, UserDto? DeletedBy);

public record ReceiptDto(string Id, string MessageId, UserDto User, DateTime Date);