using Catalog.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.Infrastructure.Persistence;

public interface ICatalogContext 
{
    DbSet<Item> Items { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync();
}

public interface ITransaction : IDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}

class UoWTransaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;

    public UoWTransaction(IDbContextTransaction  transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync() => _transaction.CommitAsync();


    public void Dispose() => _transaction.Dispose();

    public Task RollbackAsync() =>  _transaction.RollbackAsync();
}