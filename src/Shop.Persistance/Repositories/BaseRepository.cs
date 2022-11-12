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

    protected IQueryable<TEntity> ApplySpecification<TEntity>(BaseSpecification<TEntity> specification) 
        where TEntity : Entity
    {
        return GetQuery(_dbContext.Set<TEntity>(), specification);
    }

    /// <summary>
    /// Converts the specification into the IQueryable
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="inputQueryable">_dbContext.Set<TEntity>()</param>
    /// <param name="specification">Concrete specification</param>
    /// <returns></returns>
    private static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        BaseSpecification<TEntity> specification)
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQueryable;

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
            if (sortExpression == specification.SortByExpressions.First())
            {
                queryable = sortExpression.SortDirection is SortDirection.Ascending
                    ? queryable.OrderBy(sortExpression.SortBy)
                    : queryable.OrderByDescending(sortExpression.SortBy);

                continue;
            }

            queryable = sortExpression.SortDirection is SortDirection.Ascending 
                ? ((IOrderedQueryable<TEntity>)queryable).ThenBy(sortExpression.SortBy) 
                : ((IOrderedQueryable<TEntity>)queryable).ThenByDescending(sortExpression.SortBy);
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