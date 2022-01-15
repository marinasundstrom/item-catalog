using System.Linq.Expressions;

namespace Catalog.Domain;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }

    Expression<Func<T, object>> OrderBy { get; }
    string OrderByString { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    string OrderByDescendingString { get; }

    int? Skip { get; }
    int? Take { get; }
}
