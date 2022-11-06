using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;
using Shopway.Persistence.Specifications;

namespace Shopway.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly ApplicationDbContext _dbContext;

    protected BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected IQueryable<TEntity> ApplySpecification<TEntity>(Specification<TEntity> specification) 
        where TEntity : Entity
    {
        return GetQuery(_dbContext.Set<TEntity>(), specification);
    }

    //This is a static method that is able to take the concrete specification and produce the IQueryable that can execute with EF Core
    private static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQueryable;

        //TODO: why no loop here?
        //queryable = specification.IncludeExpressions.Aggregate(
        //    queryable,
        //    (current, includeExpression) => current.Include(includeExpression));

        foreach (var inlcudeExression in specification.IncludeExpressions)
        {
            queryable = queryable.Include(inlcudeExression);
        }

        foreach (var filter in specification.FilterExpressions)
        {
            queryable = queryable.Where(filter);
        }

        foreach (var sortExpression in specification.SortByExpressions)
        {
            queryable = sortExpression.SortDirection is SortDirection.Ascending 
                ? queryable.OrderBy(sortExpression.SortBy) 
                : queryable.OrderByDescending(sortExpression.SortBy);
        }

        if (specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        if (specification.IsAsNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        if (specification.IsPaginationApplied)
        {
            queryable
                .Skip(specification.PageSize * (specification.PageNumber - 1))
                .Take(specification.PageSize);
        }

        return queryable;
    }
}