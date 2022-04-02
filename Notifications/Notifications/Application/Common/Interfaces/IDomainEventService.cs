using Catalog.Notifications.Domain.Common;

namespace Catalog.Notifications.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}