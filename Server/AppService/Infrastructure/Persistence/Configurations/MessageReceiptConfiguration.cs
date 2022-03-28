
using Catalog.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

class MessageReceiptConfiguration : IEntityTypeConfiguration<MessageReceipt>
{
    public void Configure(EntityTypeBuilder<MessageReceipt> builder)
    {
        builder.ToTable("MessageReceipts");

        builder.Ignore(i => i.DomainEvents);
    }
}