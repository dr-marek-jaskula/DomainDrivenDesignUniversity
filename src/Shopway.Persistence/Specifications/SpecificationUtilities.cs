using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Persistence.Specifications;

internal static class SpecificationUtilities
{
    /// <summary>
    /// Cast specification to mapping specification
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TEntityId">EntityId type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="specification">Input specification</param>
    /// <returns>SpecificationWithMappingBase</returns>
    internal static SpecificationWithMapping<TEntity, TEntityId, TResponse> AsMappingSpecification<TEntity, TEntityId, TResponse>(this Specification<TEntity, TEntityId> specification)
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        return (SpecificationWithMapping<TEntity, TEntityId, TResponse>)specification;
    }

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

        if (specification.Filter is not null)
        {
            queryable = specification.Filter.Apply(queryable);
        }

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
    internal static IQueryable<TResponse> UseSpecification<TEntity, TEntityId, TResponse>(this IQueryable<TEntity> queryable, SpecificationWithMapping<TEntity, TEntityId, TResponse> specification)
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        if (specification.Mapping is null)
        {
            throw new ArgumentNullException($"{nameof(SpecificationWithMapping<TEntity, TEntityId, TResponse>)} must contain Select statement");
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
}