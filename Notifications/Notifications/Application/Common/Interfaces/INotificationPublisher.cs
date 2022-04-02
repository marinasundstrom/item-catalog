using Catalog.Notifications.Application.Common.Interfaces;
using Catalog.Notifications.Domain.Entities;

namespace Catalog.Notifications.Application.Common.Interfaces;

public interface INotificationPublisher
{
    Task PublishNotification(Notification notification);
}