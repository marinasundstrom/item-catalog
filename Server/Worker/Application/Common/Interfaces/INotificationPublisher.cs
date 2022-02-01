using Worker.Application.Common.Interfaces;
using Worker.Domain.Entities;

namespace Catalog.Application.Common.Interfaces;

public interface INotificationPublisher
{
    Task PublishNotification(Notification notification);
}
