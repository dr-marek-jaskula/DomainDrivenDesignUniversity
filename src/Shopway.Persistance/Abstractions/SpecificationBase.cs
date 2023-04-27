using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Enums;
using System.Linq.Expressions;

namespace Shopway.Persistence.Abstractions;

public abstract class SpecificationWithMappingBase<TEntity, TEntityId, TResposnse> : SpecificationBase<TEntity, TEntityId>
    where TEntityId : IEntityId
    where TEntity : Entity<TEntityId>
{
    public Expression<Func<TEntity, TResposnse>>? Select { get; private set; } = null;

    protected void AddSelect(Expression<Func<TEntity, TResposnse>>? select)
    {
        Select = select;
    }
}

public abstract class SpecificationBase<TEntity, TEntityId>
    where TEntityId : IEntityId
    where TEntity : Entity<TEntityId>
{
    //Flags
    public bool UseSplitQuery { get; protected set; }
    public bool UseAsNoTracking { get; protected set; }
    public bool UseAsNoTrackingWithIdentityResolution { get; protected set; }

    //Pagination
    public IPage? Page { get; private set; } = null;

    //Filters
    public IFilter<TEntity>? Filter { get; private set; } = null;
    public List<Expression<Func<TEntity, bool>>> FilterExpressions { get; } = new();

    //Includes
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    //OrderBy
    public ISortBy<TEntity>? SortBy { get; private set; } = null;
    public (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? SortByExpression { get; private set; }
    public (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? ThenByExpression { get; private set; }

    protected void AddPage(IPage page)
    {
        Page = page;
    }

    protected void AddFilters(IFilter<TEntity>? filter)
    {
        Filter = filter;
    }

    protected void AddOrder(ISortBy<TEntity>? order)
    {
        SortBy = order;
    }

    protected void AddFilters(params Expression<Func<TEntity, bool>>[] filterExpressions)
    {
        foreach (var filterExpression in filterExpressions)
        {
            FilterExpressions.Add(filterExpression);
        }
    }

    protected void AddIncludes(params Expression<Func<TEntity, object>>[] includeExpressions)
    {
        foreach (var includeExpression in includeExpressions)
        {
            IncludeExpressions.Add(includeExpression);
        }
    }

    public SpecificationBase<TEntity, TEntityId> OrderBy(Expression<Func<TEntity, object>> sortByExpression, SortDirection sortDirection)
    {
        SortByExpression = (sortByExpression, sortDirection);
        return this;
    }

    public void ThenBy(Expression<Func<TEntity, object>> thenByExpression, SortDirection sortDirection)
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
    }
}