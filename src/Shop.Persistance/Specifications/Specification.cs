using Shopway.Domain.Primitives;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications;

public abstract class Specification<TEntity>
    where TEntity : Entity
{
    protected Specification(
        bool isSplitQuery = false, 
        bool isAsNoTracking = false,
        int take = 1,
        int skip = 0,
        int pageSize = 1,
        int pageNumber = 1,
        params Expression<Func<TEntity, bool>>[]? criterias)
    {
        Criterias = criterias;
        IsSplitQuery = isSplitQuery;
        IsAsNoTracking = isAsNoTracking;
        Take = take;
        Skip = skip;
        PageSize = pageSize;
        PageNumber = pageNumber;
    }

    //Flags for: splitting, AsNoTracking
    public bool IsSplitQuery { get; protected set; }
    public bool IsAsNoTracking { get; protected set; }

    //Paging
    public int Take { get; protected set; }
    public int Skip { get; protected set; }
    public int PageSize { get; protected set; }
    public int PageNumber { get; protected set; }

    //A way to set the criteria that our entity needs to satisfy. This will match out "Where" statement. If can be nullable
    public Expression<Func<TEntity, bool>>[]? Criterias { get; }

    //We also need the way to specify the include statements. Therefore, at first we need a collection of includes:
    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    //Way to define the "OrderBy" statement. It can be nullable
    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
    }
}

