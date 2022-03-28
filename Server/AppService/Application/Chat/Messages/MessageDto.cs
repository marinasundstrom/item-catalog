using Catalog.Application.Users;

namespace Catalog.Application.Messages;

public record MessageDto(
    string Id, string? Text,
    DateTime Created, UserDto CreatedBy, DateTime? LastModified, UserDto? LastModifiedBy, DateTime? Deleted, UserDto? DeletedBy);