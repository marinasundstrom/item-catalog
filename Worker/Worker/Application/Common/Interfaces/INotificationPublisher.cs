using Catalog.Worker.Domain.Entities;

namespace Catalog.Worker.Application.Common.Interfaces;

public interface INotificationPublisher
{
    Task PublishNotification(Notification notification);
}