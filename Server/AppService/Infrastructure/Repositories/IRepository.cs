using Catalog.Domain;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Repositories;

public interface IRepository<TEntity, TKey>
    where TEntity : class, IAggregateRoot<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void Remove(TEntity entity);
}