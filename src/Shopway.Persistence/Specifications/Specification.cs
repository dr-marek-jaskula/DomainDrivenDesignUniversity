using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications;

internal sealed class SpecificationWithMapping<TEntity, TEntityId, TResponse> : Specification<TEntity, TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntity : Entity<TEntityId>
{
    internal Expression<Func<TEntity, TResponse>>? Mapping { get; private set; } = null;
    internal bool UseDistinct { get; private set; }

    internal new static SpecificationWithMapping<TEntity, TEntityId, TResponse> New()
    {
        return new SpecificationWithMapping<TEntity, TEntityId, TResponse>();
    }

    internal SpecificationWithMapping<TEntity, TEntityId, TResponse> AddMapping(Expression<Func<TEntity, TResponse>>? mapping)
    {
        Mapping = mapping;
        return this;
    }

    internal SpecificationWithMapping<TEntity, TEntityId, TResponse> ApplyDistinct()
    {
        UseDistinct = true;
        return this;
    }
}

internal class Specification<TEntity, TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntity : Entity<TEntityId>
{
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

    internal static Specification<TEntity, TEntityId> New()
    {
        return new Specification<TEntity, TEntityId>();
    }

    internal Specification<TEntity, TEntityId> AddTag(string queryTag)
    {
        QueryTag = queryTag;
        return this;
    }

    internal Specification<TEntity, TEntityId> IgnoreGlobalFilters()
    {
        UseGlobalFilters = false;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseSplitQuery()
    {
        AsSplitQuery = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseNoTracking()
    {
        AsNoTracking = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseTracking()
    {
        AsTracking = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> UseNoTrackingWithIdentityResolution()
    {
        AsNoTrackingWithIdentityResolution = true;
        return this;
    }

    internal Specification<TEntity, TEntityId> AddFilter(IFilter<TEntity>? filter)
    {
        Filter = filter;
        return this;
    }

    internal Specification<TEntity, TEntityId> AddFilter(Expression<Func<TEntity, bool>>? filterExpression)
    {
        if (filterExpression is not null)
        {
            FilterExpressions.Add(filterExpression);
        }
        return this;
    }

    internal Specification<TEntity, TEntityId> AddFilters(params Expression<Func<TEntity, bool>>[] filterExpressions)
    {
        foreach (var filterExpression in filterExpressions)
        {
            FilterExpressions.Add(filterExpression);
        }

        return this;
    }

    internal Specification<TEntity, TEntityId> AddSortBy(ISortBy<TEntity>? sortBy)
    {
        SortBy = sortBy;
        return this;
    }

    internal Specification<TEntity, TEntityId> OrderBy(Expression<Func<TEntity, object>> sortByExpression, SortDirection sortDirection)
    {
        SortByExpression = (sortByExpression, sortDirection);
        return this;
    }

    internal Specification<TEntity, TEntityId> ThenBy(Expression<Func<TEntity, object>> thenByExpression, SortDirection sortDirection)
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

    internal Specification<TEntity, TEntityId> AddIncludes(params Expression<Func<TEntity, object>>[] includeExpressions)
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
    internal Specification<TEntity, TEntityId> AddIncludesWithThenIncludesAction(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> includeAction)
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