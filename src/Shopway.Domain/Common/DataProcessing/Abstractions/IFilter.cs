using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IFilter
{
}

public interface IFilter<TEntity> : IFilter
    where TEntity : class, IEntity
{
    //If You do not want to support Like case, use this and then adjust code 
    //abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable, ILikeProvider<TEntity>? likeProvider = null);
}
