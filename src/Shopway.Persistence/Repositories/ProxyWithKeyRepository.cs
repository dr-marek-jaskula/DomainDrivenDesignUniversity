using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications;
using Shopway.Persistence.Specifications.Common;

namespace Shopway.Persistence.Repositories;

internal class ProxyWithKeyRepository<TEntity, TEntityId, TEntityKey>(ShopwayDbContext dbContext)
    : ProxyRepository<TEntity, TEntityId>(dbContext), IProxyWithKeyRepository<TEntity, TEntityId, TEntityKey>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntityKey : IUniqueKey<TEntity, TEntityKey>
{
    public async Task<TEntity> QueryByKeyAsync
    (
        TEntityKey entityKey,
        CancellationToken cancellationToken,
        Action<IIncludeBuilder<TEntity>>? buildIncludes = null
    )
    {
        var specification = CommonSpecification.Create<TEntity, TEntityId, TEntityKey>(entityKey, buildIncludes);

        return await _dbContext
            .Set<TEntity>()
            .UseSpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<TResponse> QueryByKeyAsync<TResponse>
    (
        TEntityKey entityKey,
        CancellationToken cancellationToken,
        IMapping<TEntity, TResponse>? mapping
    )
        where TResponse : class
    {
        var specificationWithMapping = CommonSpecification.Create<TEntity, TEntityId, TEntityKey, TResponse>(entityKey, mapping);

        return await _dbContext
            .Set<TEntity>()
            .UseSpecification(specificationWithMapping)
            .FirstAsync(cancellationToken);
    }
}
