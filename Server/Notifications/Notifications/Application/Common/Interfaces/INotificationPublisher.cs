using Notifications.Application.Common.Interfaces;
using Notifications.Domain.Entities;

namespace Notifications.Application.Common.Interfaces;

public interface INotificationPublisher
{
    Task PublishNotification(Notification notification);
}