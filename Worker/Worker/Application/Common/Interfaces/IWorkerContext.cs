using Catalog.Worker.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Worker.Application.Common.Interfaces;

public interface IWorkerContext
{
    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync();
}

public interface ITransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}