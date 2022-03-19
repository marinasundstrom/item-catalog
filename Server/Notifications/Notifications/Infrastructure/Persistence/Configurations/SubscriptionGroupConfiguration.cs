using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Notifications.Domain.Entities;

namespace Notifications.Infrastructure.Persistence.Configurations;

class SubscriptionGroupConfiguration : IEntityTypeConfiguration<SubscriptionGroup>
{
    public void Configure(EntityTypeBuilder<SubscriptionGroup> builder)
    {
        builder.ToTable("SubscriptionGroups");
        builder.HasQueryFilter(i => i.Deleted == null);

        builder.Ignore(i => i.DomainEvents);
    }
}