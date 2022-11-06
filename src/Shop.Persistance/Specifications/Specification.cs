using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications;

public abstract class Specification<TEntity>
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

    protected void AddOrderByWithDirection(Expression<Func<TEntity, object>> orderByExpression, SortDirection sortDirection = SortDirection.Ascending)
    {
        SortByExpressions.Add((orderByExpression, sortDirection));
    }

    //Intellisense is working bad
    protected void AddOrderByWithDirections(params ValueTuple<Expression<Func<TEntity, object>>, SortDirection>[] tuples)
    {
        foreach ((Expression<Func<TEntity, object>>, SortDirection) item in tuples)
        {
            AddOrderByWithDirection(item.Item1, item.Item2);
        }
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