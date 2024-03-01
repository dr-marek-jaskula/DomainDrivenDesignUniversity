using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Persistence.Utilities;

namespace Shopway.Persistence.Specifications;

public sealed class LikeProvider<TEntity> : ILikeProvider<TEntity>
    where TEntity : class, IEntity
{
    public IQueryable<TEntity> Apply(IQueryable<TEntity> queryable, IList<LikeEntry<TEntity>> likeEntries)
    {
        return queryable.Like(likeEntries);
    }
}