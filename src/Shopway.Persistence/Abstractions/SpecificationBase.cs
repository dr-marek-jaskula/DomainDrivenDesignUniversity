using Shopway.Domain.Enums;
using System.Linq.Expressions;
using Shopway.Domain.Utilities;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Persistence.Abstractions;

internal abstract class SpecificationWithMappingBase<TEntity, TEntityId, TResponse> : SpecificationBase<TEntity, TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
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
    where TEntityId : struct, IEntityId<TEntityId>
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
    internal bool AsTracking { get; private set; }
    internal bool AsNoTrackingWithIdentityResolution { get; private set; }
    internal bool UseGlobalFilters { get; private set; } = true;

    //Filters
    internal IFilter<TEntity>? Filter { get; private set; } = null;
    internal List<Expression<Func<TEntity, bool>>> FilterExpressions { get; } = [];

    //SortBy
    internal ISortBy<TEntity>? SortBy { get; private set; } = null;
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? SortByExpression { get; private set; }
    internal (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? ThenByExpression { get; private set; }

    //Includes
    internal List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
    internal Func<IQueryable<TEntity>, IQueryable<TEntity>>? IncludeAction { get; private set; } = null; 

    internal SpecificationBase<TEntity, TEntityId> AddTag(string queryTag)
    {
        QueryTag = queryTag;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> IgnoreGlobalFilters()
    {
        UseGlobalFilters = false;
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

    internal SpecificationBase<TEntity, TEntityId> UseTracking()
    {
        AsTracking = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> UseNoTrackingWithIdentityResolution()
    {
        AsNoTrackingWithIdentityResolution = true;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddFilter(IFilter<TEntity>? filter)
    {
        Filter = filter;
        return this;
    }

    internal SpecificationBase<TEntity, TEntityId> AddFilter(Expression<Func<TEntity, bool>>? filterExpression)
    {
        if (filterExpression is not null)
        {
            FilterExpressions.Add(filterExpression);
        }
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

    internal SpecificationBase<TEntity, TEntityId> AddSortBy(ISortBy<TEntity>? sortBy)
    {
        SortBy = sortBy;
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

    /// <summary>
    /// Use only when there is a need for ThenInclude for collection and then further include.
    /// </summary>
    /// <remarks>
    /// Example usage: .AddIncludeAction(orderHeader => orderHeader.Include(o => o.OrderLines).ThenInclude(od => od.Product)) 
    /// </remarks>
    /// <param name="includeAction"></param>
    internal SpecificationBase<TEntity, TEntityId> AddIncludesWithThenIncludesAction(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> includeAction)
    {
        var includeActionBody = includeAction.ToString();

        if (includeActionBody.NotContains("ThenInclude"))
        {
            throw new InvalidOperationException($"Input of {AddIncludesWithThenIncludesAction} must contain 'ThenInclude' call. Use {nameof(AddIncludes)} if 'ThenInclude' is not required.");
        }

        if (ContainsMethodCallDifferentFromIncludeOrThenInclude(includeActionBody))
        {
            throw new InvalidOperationException($"Input can only contain 'Include' or 'ThenInclude' calls.");
        }

        IncludeAction = includeAction.Compile();
        return this;
    }

    private static bool ContainsMethodCallDifferentFromIncludeOrThenInclude(string includeActionBody)
    {
        return includeActionBody
            .RemoveAll("ThenInclude(", "Include(")
            .Contains('(');
    }
}