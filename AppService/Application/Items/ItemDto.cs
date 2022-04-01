using Catalog.Application.Users;

namespace Catalog.Application.Items;

public record ItemDto(
    string Id, string Name, string? Description, string? Image, int CommentCount,
    DateTime Created, UserDto CreatedBy, DateTime? LastModified, UserDto? LastModifiedBy);