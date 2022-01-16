using Catalog.Domain;
using Catalog.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure;

static class QuerySpecificationExtensions
{
    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class
    {
        // fetch a Queryable that includes all expression-based includes
        var queryableResultWithIncludes = spec.Includes
            .Aggregate(query,
                (current, include) => current.Include(include));

        // modify the IQueryable to include any string-based include statements
        var secondaryResult = spec.IncludeStrings
            .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));


        IQueryable<T> tertiaryResult = secondaryResult;
        
        if(spec.Criteria is not null) 
        {
            tertiaryResult = tertiaryResult.Where(spec.Criteria);
        }

        if(spec.OrderBy is not null) 
        {
            tertiaryResult = tertiaryResult.OrderBy(spec.OrderBy);
        }

        if(spec.OrderByString is not null) 
        {
            tertiaryResult = tertiaryResult.OrderBy(spec.OrderByString, SortDirection.Ascending);
        }

        if(spec.OrderByDescending is not null) 
        {
            tertiaryResult = tertiaryResult.OrderByDescending(spec.OrderByDescending);
        }

        if(spec.OrderByDescendingString is not null) 
        {
            tertiaryResult = tertiaryResult.OrderBy(spec.OrderByDescendingString, SortDirection.Descending);
        }

        if(spec.Skip is not null) 
        {
            tertiaryResult = tertiaryResult.Skip(spec.Skip.GetValueOrDefault());
        }

        if(spec.Take is not null) 
        {
            tertiaryResult = tertiaryResult.Take(spec.Take.GetValueOrDefault());
        }

        // return the result of the query using the specification's criteria expression
        return tertiaryResult;
    }
}