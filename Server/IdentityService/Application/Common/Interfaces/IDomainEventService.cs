using Catalog.IdentityService.Domain.Common;

namespace Catalog.IdentityService.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}