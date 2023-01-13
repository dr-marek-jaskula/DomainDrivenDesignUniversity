using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.Enums;
using System.Linq.Expressions;

namespace Shopway.Persistence.Abstractions;

public abstract class BaseSpecification<TEntity, TEntityId>
    where TEntityId : IEntityId<TEntityId>, new()
    where TEntity : Entity<TEntityId>
{
    //Flags
    public bool IsSplitQuery { get; protected set; }

    //Filters and Includes
    public IFilter<TEntity>? Filter { get; private set; } = null;
    public List<Expression<Func<TEntity, bool>>> FilterExpressions { get; } = new();
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    //OrderBy
    public ISortBy? SortBy { get; private set; } = null;
    public (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? SortByExpression { get; private set; }
    public (Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)? ThenByExpression { get; private set; }

    protected void AddOrder(ISortBy? order)
    {
        SortBy = order;
    }

    protected void AddFilters(IFilter<TEntity>? filter)
    {
        Filter = filter;
    }

    public BaseSpecification<TEntity, TEntityId> OrderBy(Expression<Func<TEntity, object>> sortByExpression, SortDirection sortDirection)
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
}