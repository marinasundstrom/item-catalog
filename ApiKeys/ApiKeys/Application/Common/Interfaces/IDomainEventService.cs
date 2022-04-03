using Catalog.ApiKeys.Domain.Common;

namespace Catalog.ApiKeys.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}