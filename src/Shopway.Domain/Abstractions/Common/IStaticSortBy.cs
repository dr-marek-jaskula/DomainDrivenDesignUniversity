namespace Shopway.Domain.Abstractions.Common;

public interface IStaticSortBy<TEntity> : ISortBy
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}