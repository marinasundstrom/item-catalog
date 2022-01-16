using Catalog.Domain.Entities;

namespace Catalog.Domain;

public class PagedItemsSpecification : BaseSpecification<Item>
{
    public PagedItemsSpecification(int page, int pageSize) 
    {
        //AddOrderBy(x => x.Created);
        ApplyPagination(page, pageSize);
    }

    public new PagedItemsSpecification AddOrderBy(string propertyPath) 
    {
        base.AddOrderBy(propertyPath);

        return this;
    }
    
    public new PagedItemsSpecification AddOrderByDescending(string propertyPath) 
    {
        base.AddOrderByDescending(propertyPath);

        return this;
    }
}