using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface ILikeProvider<TEntity>
    where TEntity : class, IEntity
{
    IQueryable<TEntity> Apply(IQueryable<TEntity> queryable, IList<LikeEntry<TEntity>> likeEntries);
}