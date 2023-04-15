using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Utilities;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Abstractions;

public abstract class RepositoryBase
{
    protected readonly ShopwayDbContext _dbContext;

    protected RepositoryBase(ShopwayDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected IQueryable<TEntity> ApplySpecification<TEntity, TEntityId>(SpecificationBase<TEntity, TEntityId> specification)
        where TEntityId : IEntityId
        where TEntity : Entity<TEntityId>
    {
        return GetQuery(_dbContext.Set<TEntity>(), specification);
    }

    /// <summary>
    /// Converts the specification into the IQueryable
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="inputQueryable">_dbContext.Set<TEntity>()</param>
    /// <param name="specification">Concrete specification</param>
    /// <returns>Query</returns>
    private static IQueryable<TEntity> GetQuery<TEntity, TEntityId>(IQueryable<TEntity> queryable, SpecificationBase<TEntity, TEntityId> specification)
        where TEntityId : IEntityId
        where TEntity : Entity<TEntityId>
    {
        foreach (var includeExpression in specification.IncludeExpressions)
        {
            queryable = queryable.Include(includeExpression);
        }

        if (specification.Filter is not null)
        {
            queryable = specification.Filter.Apply(queryable);
        }
        else if (specification.FilterExpressions.IsNotNullOrEmpty())
        {
            foreach (var filter in specification.FilterExpressions)
            {
                queryable = queryable.Where(filter);
            }
        }

        if (specification.SortBy is not null)
        {
            queryable = specification.SortBy.Apply(queryable);
        }
        else if (specification.SortByExpression is not null and var sort)
        {
            queryable = queryable.OrderBy(sort.Value.SortBy, sort.Value.SortDirection);

            if (specification.ThenByExpression is not null and var then)
            {
                queryable = ((IOrderedQueryable<TEntity>)queryable).ThenBy(then.Value.SortBy, then.Value.SortDirection);
            }
        }

        if (specification.UseSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        if (specification.UseAsNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        if (specification.UseAsNoTrackingWithIdentityResolution)
        {
            queryable = queryable.AsNoTrackingWithIdentityResolution();
        }

        return queryable;
    }
}