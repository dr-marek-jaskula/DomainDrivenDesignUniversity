using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Enums;
using System.Linq.Expressions;

namespace Shopway.Persistence.Abstractions;

public abstract class SpecificationWithMappingBase<TEntity, TEntityId, TResponse> : SpecificationBase<TEntity, TEntityId>
    where TEntityId : IEntityId
    where TEntity : Entity<TEntityId>
{
    internal Expression<Func<TEntity, TResponse>>? Select { get; private set; } = null;

    internal SpecificationWithMappingBase<TEntity, TEntityId, TResponse> AddSelect(Expression<Func<TEntity, TResponse>>? select)
    {
        Select = select;
        return this;
    }
}

public abstract class SpecificationBase<TEntity, TEntityId>
    where TEntityId : IEntityId
    where TEntity : Entity<TEntityId>
{
    //Flags
    internal bool AsSplitQuery { get; private set; }
    internal bool AsNoTracking { get; private set; }
    internal bool AsNoTrackingWithIdentityResolution { get; private set; }

    //Pagination
    internal IPage? Page { get; private set; } = null;

    //Filters
    internal IFilter<TEntity>? Filter { get; private set; } = null;
    internal List<Expression<Func<TEntity, bool>>> FilterExpressions { get; } = new();

    //Includes
    internal List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    //OrderBy
    internal ISortBy<TEntity>? SortBy { get; private set; } = null;
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? SortByExpression { get; private set; }
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? ThenByExpression { get; private set; }

    internal SpecificationBase<TEntity, TEntityId> UseSplitQuery()
    {
        AsSplitQuery = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> UseAsNoTracking()
    {
        AsNoTracking = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> UseAsNoTrackingWithIdentityResolution()
    {
        AsNoTrackingWithIdentityResolution = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddPage(IPage page)
    {
        Page = page;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddFilter(IFilter<TEntity>? filter)
    {
        Filter = filter;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddOrder(ISortBy<TEntity>? order)
    {
        SortBy = order;
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

    internal SpecificationBase<TEntity, TEntityId> AddIncludes(params Expression<Func<TEntity, object>>[] includeExpressions)
    {
        foreach (var includeExpression in includeExpressions)
        {
            IncludeExpressions.Add(includeExpression);
        }

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
}