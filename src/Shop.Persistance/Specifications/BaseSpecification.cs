using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications;

public interface ISortBy<TEntity>
{
    public ISortBy<TEntity> ThenByWithDirection(Expression<Func<TEntity, object>> orderByExpression, SortDirection sortDirection);
}

public abstract class BaseSpecification<TEntity> : ISortBy<TEntity>
    where TEntity : Entity
{
    //Flags
    public bool IsSplitQuery { get; protected set; }
    public bool IsAsNoTracking { get; protected set; }

    //Pagination
    public bool IsPaginationApplied { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public int PageSize { get; private set; }
    public int PageNumber { get; private set; }

    //Filters and Includes
    public List<Expression<Func<TEntity, bool>>> FilterExpressions { get; } = new();
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    //OrderBy
    public List<(Expression<Func<TEntity, object>> SortBy, SortDirection SortDirection)> SortByExpressions { get; private set; } = new();

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

    /// <summary>
    /// Use this method only once. Afterwards, use ThenByWithDirection to chain sorting.
    /// </summary>
    /// <param name="orderByExpression"></param>
    /// <param name="sortDirection"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected ISortBy<TEntity> OrderByWithDirection(Expression<Func<TEntity, object>> orderByExpression, SortDirection sortDirection = SortDirection.Ascending)
    {
        if (SortByExpressions.Any())
        {
            throw new InvalidOperationException($"{nameof(OrderByWithDirection)} for {nameof(TEntity)} was called twice. Use {nameof(ISortBy<TEntity>.ThenByWithDirection)} to chain sorting.");
        }

        SortByExpressions.Add((orderByExpression, sortDirection));
        return this;
    }

    ISortBy<TEntity> ISortBy<TEntity>.ThenByWithDirection(Expression<Func<TEntity, object>> orderByExpression, SortDirection sortDirection)
    {
        SortByExpressions.Add((orderByExpression, sortDirection));
        return this;
    }

    protected void AddPagination(int take, int skip, int pageSize, int pageNumber)
    {
        Take = take;
        Skip = skip;
        PageSize = pageSize;
        PageNumber = pageNumber;

        IsPaginationApplied = true;
    }
}