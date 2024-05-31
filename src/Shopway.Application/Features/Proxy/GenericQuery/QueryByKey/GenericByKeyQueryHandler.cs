using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;

internal sealed class GenericByKeyQueryHandler<TEntity, TEntityId, TEntityKey>(IProxyWithKeyRepository<TEntity, TEntityId, TEntityKey> proxyWithKeyRepository)
    : IQueryHandler<GenericByKeyQuery<TEntity, TEntityId, TEntityKey>, DataTransferObjectResponse>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntityKey : IUniqueKey<TEntity, TEntityKey>
{
    private readonly IProxyWithKeyRepository<TEntity, TEntityId, TEntityKey> _proxyWithKeyRepository = proxyWithKeyRepository;

    public async Task<IResult<DataTransferObjectResponse>> Handle(GenericByKeyQuery<TEntity, TEntityId, TEntityKey> query, CancellationToken cancellationToken)
    {
        var entityDto = await _proxyWithKeyRepository
            .QueryByKeyAsync(query.EntityKey, cancellationToken, query.Mapping);

        return entityDto
            .ToResponse()
            .ToResult();
    }
}
