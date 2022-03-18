using Notifications.Domain.Common;

namespace Notifications.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}