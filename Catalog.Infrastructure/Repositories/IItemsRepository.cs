using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Repositories;

public interface IItemsRepository : IRepository<Item, string>
{
}
