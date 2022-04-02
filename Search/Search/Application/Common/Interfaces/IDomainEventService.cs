using Catalog.Search.Domain.Common;

namespace Catalog.Search.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}