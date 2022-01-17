using System.Linq.Expressions;

namespace Catalog.Domain;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; set; }

    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();

    public Expression<Func<T, object>> OrderBy { get; private set; }
    public string OrderByString { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public string OrderByDescendingString { get; private set; }

    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    
    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
 
    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderBy(string propertyPath)
    {
        OrderByString = propertyPath;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void AddOrderByDescending(string propertyPath)
    {
        OrderByDescendingString = propertyPath;
    }

    protected void ApplyPagination(int page, int pageSize = 10)
    {
        Skip = pageSize * (page - 1);
        Take = pageSize;
    }
}
