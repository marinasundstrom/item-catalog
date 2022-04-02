using Catalog.Search.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Search.Application.Common.Interfaces;

public interface ISearchContext
{
    DbSet<Item> Items { get; }

    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync();
}

public interface ITransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}