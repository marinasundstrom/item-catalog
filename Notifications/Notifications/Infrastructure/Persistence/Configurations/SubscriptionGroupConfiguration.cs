using Catalog.Notifications.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Notifications.Infrastructure.Persistence.Configurations;

class SubscriptionGroupConfiguration : IEntityTypeConfiguration<SubscriptionGroup>
{
    public void Configure(EntityTypeBuilder<SubscriptionGroup> builder)
    {
        builder.ToTable("SubscriptionGroups");
        builder.HasQueryFilter(i => i.Deleted == null);

        builder.Ignore(i => i.DomainEvents);
    }
}