using Catalog.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.Application.Common.Interfaces;

public interface ICatalogContext
{
    DbSet<Item> Items { get; }

    DbSet<Comment> Comments { get; }

    DbSet<Conversation> Conversations { get; }

    DbSet<ConversationParticipant> ConversationParticipants { get; }

    DbSet<Message> Messages { get; }

    DbSet<MessageReceipt> MessageReceipts { get; }

    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync();
}

public interface ITransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}