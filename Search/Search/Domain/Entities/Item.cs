using System;

using Catalog.Search.Domain.Common;

namespace Catalog.Search.Domain.Entities;

public class Item : AuditableEntity, ISoftDelete, IHasDomainEvent
{
    protected Item()
    {

    }

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;

    public DateTime? Deleted { get; set; }
    public string? DeletedById { get; set; }
    public User? DeletedBy { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}