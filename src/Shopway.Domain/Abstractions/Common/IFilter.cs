namespace Shopway.Domain.Abstractions.Common;

public interface IFilter
{
}

public interface IFilter<TEntity> : IFilter
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}