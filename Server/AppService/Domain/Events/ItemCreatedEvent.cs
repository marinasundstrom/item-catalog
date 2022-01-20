using Catalog.Domain.Common;

namespace Catalog.Domain.Events;

public class ItemCreatedEvent : DomainEvent
{
    public ItemCreatedEvent(string id)
    {
        this.Id = id;
    }

    public string Id { get; }
}