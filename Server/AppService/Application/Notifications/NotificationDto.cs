namespace Catalog.Application.Notifications;

public record NotificationDto(
    string Id, DateTime Published, string Title, string? Text, bool IsRead,
    DateTime Created, string CreatedBy, DateTime? LastModified, string? LastModifiedBy);