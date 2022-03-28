using Catalog.Application.Users;

namespace Catalog.Application.Messages;

public record MessageDto(
    string Id, string? Text,
    IEnumerable<ReceiptDto> Receipts,
    DateTime Created, UserDto CreatedBy, DateTime? LastModified, UserDto? LastModifiedBy, DateTime? Deleted, UserDto? DeletedBy);

public record ReceiptDto(UserDto User, DateTime Date);