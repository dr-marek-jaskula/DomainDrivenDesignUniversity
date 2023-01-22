using Shopway.Domain.BaseTypes;

namespace Shopway.Domain.Abstractions;

public interface ISortBy
{
}

public interface ISortBy<TEntity> : ISortBy
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}