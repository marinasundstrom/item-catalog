using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.Infrastructure.Repositories;

public interface IUnitOfWork 
{
    IItemsRepository Items { get; }

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

    public Task CommitAsync() =>  _transaction.CommitAsync();


    public void Dispose()=>  _transaction.Dispose();

    public Task RollbackAsync()=>  _transaction.RollbackAsync();
}