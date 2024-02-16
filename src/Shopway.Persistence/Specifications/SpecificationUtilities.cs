using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Persistence.Utilities;

namespace Shopway.Persistence.Specifications;

internal static class SpecificationUtilities
{
    /// <summary>
    /// Apply a specification and return a queryable
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <param name="queryable">_dbContext.Set<TEntity>()</param>
    /// <param name="specification">Input specification</param>
    /// <returns>Queryable</returns>
    internal static IQueryable<TEntity> UseSpecification<TEntity, TEntityId>(this IQueryable<TEntity> queryable, Specification<TEntity, TEntityId> specification)
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        return queryable
            .ApplyIncludes(specification)
            .ApplyFilters(specification)
            .ApplyLikes(specification)
            .ApplySorting(specification)
            .ApplyQueryOptions(specification);
    }

    /// <summary>
    /// Apply a specification and return a queryable of the given response type
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <typeparam name="Output">Output type</typeparam>
    /// <param name="queryable">_dbContext.Set<TEntity>()</param>
    /// <param name="specification">Input specification</param>
    /// <returns>Queryable</returns>
    internal static IQueryable<Output> UseSpecification<TEntity, TEntityId, Output>(this IQueryable<TEntity> queryable, SpecificationWithMapping<TEntity, TEntityId, Output> specification)
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        if (specification.Mapping is null)
        {
            throw new ArgumentNullException($"{nameof(SpecificationWithMapping<TEntity, TEntityId, Output>)} must contain Select statement");
        }

        var queryableWithMapping = queryable
            .UseSpecification((Specification<TEntity, TEntityId>)specification)
            .Select(specification.Mapping);

        if (specification.UseDistinct)
        {
            queryableWithMapping = queryableWithMapping.Distinct();
        }

        return queryableWithMapping;
    }

    private static IQueryable<TEntity> ApplyIncludes<TEntity, TEntityId>(this IQueryable<TEntity> queryable, Specification<TEntity, TEntityId> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        if (specification.IncludeAction is not null)
        {
            queryable = specification.IncludeAction(queryable);
        }

        foreach (var includeExpression in specification.IncludeExpressions)
        {
            queryable = queryable.Include(includeExpression);
        }

        foreach (var includeString in specification.IncludeStrings)
        {
            queryable = queryable.Include(includeString);
        }

        return queryable;
    }

    private static IQueryable<TEntity> ApplyFilters<TEntity, TEntityId>(this IQueryable<TEntity> queryable, Specification<TEntity, TEntityId> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {

        foreach (var filter in specification.FilterExpressions)
        {
            queryable = queryable.Where(filter);
        }

        if (specification.Filter is not null)
        {
            queryable = specification.Filter.Apply(queryable);
        }

        return queryable;
    }

    private static IQueryable<TEntity> ApplyLikes<TEntity, TEntityId>(this IQueryable<TEntity> queryable, Specification<TEntity, TEntityId> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return queryable.Like<TEntity, TEntityId>(specification.LikeEntries);
    }

    private static IQueryable<TEntity> ApplySorting<TEntity, TEntityId>(this IQueryable<TEntity> queryable, Specification<TEntity, TEntityId> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        if (specification.SortBy is not null)
        {
            queryable = specification.SortBy.Apply(queryable);
        }

        if (specification.SortByExpression is not null and var sort)
        {
            queryable = queryable.OrderBy(sort.Value.SortBy, sort.Value.SortDirection);

            if (specification.ThenByExpression is not null and var then)
            {
                queryable = ((IOrderedQueryable<TEntity>)queryable).ThenBy(then.Value.SortBy, then.Value.SortDirection);
            }
        }

        return queryable;
    }

    private static IQueryable<TEntity> ApplyQueryOptions<TEntity, TEntityId>(this IQueryable<TEntity> queryable, Specification<TEntity, TEntityId> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
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
}