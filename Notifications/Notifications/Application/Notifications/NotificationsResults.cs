
using Catalog.Notifications.Application.Common.Models;

namespace Catalog.Notifications.Application.Notifications;

public record class NotificationsResults(IEnumerable<NotificationDto> Items, int? UnreadNotificationsCount, int TotalCount)
    : Results<NotificationDto>(Items, TotalCount);