using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface ILikeProvider<TEntity>
    where TEntity : class, IEntity
{
    IQueryable<TEntity> Apply(IQueryable<TEntity> queryable, IList<LikeEntry<TEntity>> likeEntries);
    Expression CreateLikeExpression(ParameterExpression parameter, string property, string likeTerm);
}