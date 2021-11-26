using System;

using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>().HasQueryFilter(i => i.DeletedAt == null);
    }

    public DbSet<Item> Items { get; set; } = null!;
}

public class Item
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
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}