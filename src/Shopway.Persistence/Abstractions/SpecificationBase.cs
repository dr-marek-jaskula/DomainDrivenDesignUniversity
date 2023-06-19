using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Common;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Enums;
using System.Linq.Expressions;

namespace Shopway.Persistence.Abstractions;

internal abstract class SpecificationWithMappingBase<TEntity, TEntityId, TResponse> : SpecificationBase<TEntity, TEntityId>
    where TEntityId : struct, IEntityId
    where TEntity : Entity<TEntityId>
{
    internal Expression<Func<TEntity, TResponse>>? Mapping { get; private set; } = null;

    internal SpecificationWithMappingBase<TEntity, TEntityId, TResponse> AddMapping(Expression<Func<TEntity, TResponse>>? mapping)
    {
        Mapping = mapping;
        return this;
    }
}

internal abstract class SpecificationBase<TEntity, TEntityId>
    where TEntityId : struct, IEntityId
    where TEntity : Entity<TEntityId>
{
    protected SpecificationBase()
    {
    }

    //Tag
    internal string? QueryTag { get; private set; }

    //Flags
    internal bool AsSplitQuery { get; private set; }
    internal bool AsNoTracking { get; private set; }
    internal bool AsNoTrackingWithIdentityResolution { get; private set; }

    //Filters
    internal IStaticFilter<TEntity>? StaticFilter { get; private set; } = null;
    internal IDynamicFilter<TEntity>? DynamicFilter { get; private set; } = null;
    internal List<Expression<Func<TEntity, bool>>> FilterExpressions { get; } = new();

    //SortBy
    internal IDynamicSortBy<TEntity>? DynamicSortBy { get; private set; } = null;
    internal IStaticSortBy<TEntity>? StaticSortBy { get; private set; } = null;
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? SortByExpression { get; private set; }
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? ThenByExpression { get; private set; }

    //Includes
    internal List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    internal SpecificationBase<TEntity, TEntityId> AddTag(string queryTag)
    {
        QueryTag = queryTag;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> UseSplitQuery()
    {
        AsSplitQuery = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> UseNoTracking()
    {
        AsNoTracking = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> UseNoTrackingWithIdentityResolution()
    {
        AsNoTrackingWithIdentityResolution = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddFilter(IStaticFilter<TEntity>? filter)
    {
        StaticFilter = filter;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddFilter(IDynamicFilter<TEntity>? filter)
    {
        DynamicFilter = filter;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddFilters(params Expression<Func<TEntity, bool>>[] filterExpressions)
    {
        foreach (var filterExpression in filterExpressions)
        {
            FilterExpressions.Add(filterExpression);
        }

        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddSortBy(IDynamicSortBy<TEntity>? sortBy)
    {
        DynamicSortBy = sortBy;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddSortBy(IStaticSortBy<TEntity>? sortBy)
    {
        StaticSortBy = sortBy;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> OrderBy(Expression<Func<TEntity, object>> sortByExpression, SortDirection sortDirection)
    {
        SortByExpression = (sortByExpression, sortDirection);
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> ThenBy(Expression<Func<TEntity, object>> thenByExpression, SortDirection sortDirection)
    {
        if (SortByExpression is null)
        {
            throw new InvalidOperationException($"{nameof(SortByExpression)} should be specified before {nameof(ThenByExpression)}");
        }

        if (ThenByExpression is not null)
        {
            throw new InvalidOperationException($"{nameof(ThenByExpression)} can be specified once");
        }

        ThenByExpression = (thenByExpression, sortDirection);
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddIncludes(params Expression<Func<TEntity, object>>[] includeExpressions)
    {
        foreach (var includeExpression in includeExpressions)
        {
            IncludeExpressions.Add(includeExpression);
        }

        return this;
    }
}