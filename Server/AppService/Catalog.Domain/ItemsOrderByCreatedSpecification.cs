using System.Linq.Expressions;
using Catalog.Domain.Entities;

namespace Catalog.Domain;

public class ItemsOrderByCreatedSpecification : BaseSpecification<Item>
{
    public ItemsOrderByCreatedSpecification()
    {
        AddOrderBy(x => x.Created);
    }
}

public class OrderBySpecification<T> : BaseSpecification<T>
{
    public OrderBySpecification(Expression<Func<T, object>> expression)
    {
        AddOrderBy(expression);
    }

    public OrderBySpecification(string propertyPath)
    {
        AddOrderBy(propertyPath);
    }
}
