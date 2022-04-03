
using Catalog.ApiKeys.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.ApiKeys.Infrastructure.Persistence.Configurations;

class ItemConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.ToTable("ApiKeys");
        builder.HasQueryFilter(i => i.Deleted == null);

        builder.HasIndex(nameof(ApiKey.Key));

        builder.Ignore(i => i.DomainEvents);
    }
}
