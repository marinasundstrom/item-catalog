using System;

using Notifications.Application.Common.Models;

namespace Notifications.Application.Notifications;

public record class NotificationsResults(IEnumerable<NotificationDto> Items, int? UnreadNotificationsCount, int TotalCount)
    : Results<NotificationDto>(Items, TotalCount);