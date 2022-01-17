using Catalog.Domain;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

class ItemsRepository : IItemsRepository
{
    private readonly CatalogContext _context;

    public ItemsRepository(CatalogContext context)
    {
        _context = context;
    }

    public IUnitOfWork Context => _context;

    public void Add(Item entity)
    {
        _context.Items.Add(entity);
    }

    public async Task<IEnumerable<Item>> GetAllAsync(ISpecification<Item> specification, CancellationToken cancellationToken = default)
    {
        return await _context.Items
            .Specify(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<Item?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Items.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<int> CountAsync(ISpecification<Item> specification, CancellationToken cancellationToken = default)
    {
        return await _context.Items
            .Specify(specification)
            .CountAsync(cancellationToken);
    }

    public void Remove(Item entity)
    {
        _context.Items.Remove(entity);
    }
}
