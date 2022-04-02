using Catalog.Worker.Domain.Common;

namespace Catalog.Worker.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}