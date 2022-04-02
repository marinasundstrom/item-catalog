using Catalog.Messenger.Domain.Common;

namespace Catalog.Messenger.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}