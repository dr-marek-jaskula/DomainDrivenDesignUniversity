using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public interface IProxyWithKeyRepository<TEntity, TEntityId, TEntityKey> : IProxyRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntityKey : IUniqueKey<TEntity, TEntityKey>
{
    Task<TEntity> QueryByKeyAsync(TEntityKey entityKey, CancellationToken cancellationToken, Action<IIncludeBuilder<TEntity>>? buildIncludes = null);
    Task<TResponse> QueryByKeyAsync<TResponse>(TEntityKey entityKey, CancellationToken cancellationToken, IMapping<TEntity, TResponse>? mapping) where TResponse : class;
}
