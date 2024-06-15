using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IMapping
{
}

public interface IMapping<TEntity, TOutput> : IMapping
    where TEntity : class, IEntity
{
    abstract IQueryable<TOutput> Apply(IQueryable<TEntity> queryable);
}
