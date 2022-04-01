using Messenger.Domain.Common;

namespace Messenger.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}