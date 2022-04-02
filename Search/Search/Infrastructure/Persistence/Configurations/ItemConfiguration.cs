using System;

using Catalog.Search.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Search.Infrastructure.Persistence.Configurations;

class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");
        builder.HasQueryFilter(i => i.Deleted == null);

        builder.HasIndex(nameof(Item.Name), nameof(Item.Description));

        builder.Ignore(i => i.DomainEvents);
    }
}
