using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public interface IProxyRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    Task<(List<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>(ICursorPage page, CancellationToken cancellationToken, IFilter<TEntity>? filter = null, List<LikeEntry<TEntity>>? likes = null, ISortBy<TEntity>? sort = null, IMapping<TEntity, TResponse>? mapping = null, Expression<Func<TEntity, TResponse>>? mappingExpression = null, Action<IIncludeBuilder<TEntity>>? buildIncludes = null) where TResponse : class, IHasCursor;
    Task<(List<TResponse> Responses, int TotalCount)> PageAsync<TResponse>(IOffsetPage page, CancellationToken cancellationToken, IFilter<TEntity>? filter = null, List<LikeEntry<TEntity>>? likes = null, ISortBy<TEntity>? sort = null, IMapping<TEntity, TResponse>? mapping = null, Expression<Func<TEntity, TResponse>>? mappingExpression = null, Action<IIncludeBuilder<TEntity>>? buildIncludes = null);
    Task<TEntity> QueryByIdAsync(TEntityId entityId, CancellationToken cancellationToken, Action<IIncludeBuilder<TEntity>>? buildIncludes = null);
    Task<TResponse> QueryByIdAsync<TResponse>(TEntityId entityId, CancellationToken cancellationToken, IMapping<TEntity, TResponse>? mapping) where TResponse : class;
}
