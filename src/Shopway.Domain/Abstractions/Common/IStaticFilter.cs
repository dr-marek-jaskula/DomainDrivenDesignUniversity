namespace Shopway.Domain.Abstractions.Common;

public interface IStaticFilter<TEntity> : IFilter
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}