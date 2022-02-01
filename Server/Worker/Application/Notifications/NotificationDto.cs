namespace Worker.Application.Notifications;

public record NotificationDto(
    string Id, string Title, string? Text, string? Link, string? UserId, bool IsRead, DateTime? Published, DateTime? ScheduledFor,
    DateTime Created, string CreatedBy, DateTime? LastModified, string? LastModifiedBy);