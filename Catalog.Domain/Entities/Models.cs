using System;

namespace Catalog.Domain.Entities;

public class Item : AuditableEntity, IAggregateRoot<string>, ISoftDelete
{
    public Item()
    {

    }

    public Item(string id, string name, string? description = null)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public string? Image { get; set; } = null!;


    public DateTime? Deleted { get; set; }
    public string? DeletedBy { get; set; }
}
