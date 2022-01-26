namespace Catalog.Application.Models;

public record ItemDto(
    string Id, string Name, string? Description, string? Image, int CommentCount,
    DateTime Created, string CreatedBy, DateTime? LastModified, string? LastModifiedBy);
