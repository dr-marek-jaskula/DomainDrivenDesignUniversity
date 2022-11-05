using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Shopway.Domain.Primitives;
using System.Linq;

namespace Shopway.Persistence.Specifications;

//This is a static class that is able to take the concrete specification and produce the IQueryable that can execute with EF Core
public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable, 
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if (specification.Criterias is not null)
        {
            foreach (var criteria in specification.Criterias)
            {
                queryable = queryable.Where(criteria);
            }
        }

        queryable = specification.IncludeExpressions.Aggregate(
            queryable, 
            (current, includeExpression) => current.Include(includeExpression));

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        if (specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        if (specification.IsAsNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        if (specification.Skip is not 0 && specification.Take is not 1)
        {
            queryable
                .Skip(specification.PageSize * (specification.PageNumber - 1))
                .Take(specification.PageSize);
        }

        //TODO: how to mage PageSize from specification and PageNumber

        return queryable;
    }
}

