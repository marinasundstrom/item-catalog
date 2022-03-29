using Catalog.Application.Users;

namespace Catalog.Application.Messages;

public record MessageDto(
    string Id, string? Text,
    IEnumerable<ReceiptDto> Receipts,
    DateTime Sent, UserDto SentBy, DateTime? LastModified, UserDto? LastModifiedBy, DateTime? Deleted, UserDto? DeletedBy);

public record ReceiptDto(string Id, UserDto User, DateTime Date);