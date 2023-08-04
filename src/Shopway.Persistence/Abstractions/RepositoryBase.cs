using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Utilities;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Abstractions;

public abstract class RepositoryBase
{
    private protected readonly ShopwayDbContext _dbContext;

    private protected RepositoryBase(ShopwayDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Apply a specification and return a queryable
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <param name="queryable">_dbContext.Set<TEntity>()</param>
    /// <param name="specification">Input specification</param>
    /// <returns>Queryable</returns>
    private protected IQueryable<TEntity> UseSpecification<TEntity, TEntityId>(SpecificationBase<TEntity, TEntityId> specification)
        where TEntityId : struct, IEntityId
        where TEntity : Entity<TEntityId>
    {
        IQueryable<TEntity> queryable = _dbContext.Set<TEntity>();

        if (specification.IncludeAction is not null)
        {
            queryable = specification.IncludeAction(queryable);
        }

        foreach (var includeExpression in specification.IncludeExpressions)
        {
            queryable = queryable.Include(includeExpression);
        }

        if (specification.FilterExpressions.NotNullOrEmpty())
        {
            foreach (var filter in specification.FilterExpressions)
            {
                queryable = queryable.Where(filter);
            }
        }

        if (specification.StaticFilter is not null)
        {
            queryable = specification.StaticFilter.Apply(queryable);
        }

        if (specification.DynamicFilter is not null)
        {
            queryable = specification.DynamicFilter.Apply(queryable);
        }

        if (specification.DynamicSortBy is not null)
        {
            queryable = specification.DynamicSortBy.Apply(queryable);
        }

        if (specification.StaticSortBy is not null)
        {
            queryable = specification.StaticSortBy.Apply(queryable);
        }

        if (specification.SortByExpression is not null and var sort)
        {
            queryable = queryable.OrderBy(sort.Value.SortBy, sort.Value.SortDirection);

            if (specification.ThenByExpression is not null and var then)
            {
                queryable = ((IOrderedQueryable<TEntity>)queryable).ThenBy(then.Value.SortBy, then.Value.SortDirection);
            }
        }

        if (specification.QueryTag is not null)
        {
            queryable = queryable.TagWith(specification.QueryTag);
        }

        if (specification.AsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        if (specification.AsNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        if (specification.AsTracking)
        {
            queryable = queryable.AsTracking();
        }

        if (specification.AsNoTrackingWithIdentityResolution)
        {
            queryable = queryable.AsNoTrackingWithIdentityResolution();
        }

        if (specification.UseGlobalFilters is false)
        {
            queryable = queryable.IgnoreQueryFilters();
        }

        return queryable;
    }

    /// <summary>
    /// Apply a specification and return a queryable of the given response type
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="queryable">_dbContext.Set<TEntity>()</param>
    /// <param name="specification">Input specification</param>
    /// <returns>Queryable</returns>
    private protected IQueryable<TResponse> UseSpecification<TEntity, TEntityId, TResponse>(SpecificationWithMappingBase<TEntity, TEntityId, TResponse> specification)
        where TEntityId : struct, IEntityId
        where TEntity : Entity<TEntityId>
    {
        if (specification.Mapping is null)
        {
            throw new ArgumentNullException($"SpecificationWithMappingBase must contain Select statement");
        }

        return UseSpecification((SpecificationBase<TEntity, TEntityId>)specification)
            .Select(specification.Mapping);
    }
}