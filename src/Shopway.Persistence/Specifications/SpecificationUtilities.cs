using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
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
    /// <typeparam name="TOutput">Output type</typeparam>
    /// <param name="queryable">_dbContext.Set<TEntity>()</param>
    /// <param name="specification">Input specification</param>
    /// <returns>Queryable</returns>
    internal static IQueryable<TOutput> UseSpecification<TEntity, TEntityId, TOutput>(this IQueryable<TEntity> queryable, SpecificationWithMapping<TEntity, TEntityId, TOutput> specification)
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        return queryable
            .UseSpecification((Specification<TEntity, TEntityId>)specification)
            .ApplyMapping(specification)
            .ApplyDistinct(specification);
    }

    private static IQueryable<TOutput> ApplyMapping<TEntity, TEntityId, TOutput>(this IQueryable<TEntity> queryable, SpecificationWithMapping<TEntity, TEntityId, TOutput> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        if (specification.MappingExpression is not null && specification.Mapping is not null)
        {
            throw new InvalidOperationException($"{nameof(SpecificationWithMapping<TEntity, TEntityId, TOutput>)} must contain single Mapping.");
        }

        if (specification.MappingExpression is not null)
        {
            return queryable
                .Select(specification.MappingExpression);
        }

        if (specification.Mapping is not null)
        {
            return specification.Mapping.Apply(queryable);
        }

        throw new ArgumentNullException($"{nameof(SpecificationWithMapping<TEntity, TEntityId, TOutput>)} must contain Mapping.");
    }

    private static IQueryable<TOutput> ApplyDistinct<TEntity, TEntityId, TOutput>(this IQueryable<TOutput> queryable, SpecificationWithMapping<TEntity, TEntityId, TOutput> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        if (specification.UseDistinct)
        {
            return queryable.Distinct();
        }

        return queryable;
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

        foreach (var includeEntry in specification.IncludeEntries)
        {
            queryable = includeEntry.IncludeType switch
            {
                IncludeType.Include => queryable.AddInclude<TEntity, TEntityId>(includeEntry),
                IncludeType.ThenInclude => queryable.AddThenInclude<TEntity, TEntityId>(includeEntry),
                _ => throw new NotSupportedException()
            };
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
        return queryable.Like(specification.LikeEntries);
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