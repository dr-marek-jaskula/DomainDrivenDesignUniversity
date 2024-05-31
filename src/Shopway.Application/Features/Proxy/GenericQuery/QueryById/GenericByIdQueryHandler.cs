using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryById;

internal sealed class GenericByIdQueryHandler<TEntity, TEntityId>(IProxyRepository<TEntity, TEntityId> proxyRepository)
    : IQueryHandler<GenericByIdQuery<TEntity, TEntityId>, DataTransferObjectResponse>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    private readonly IProxyRepository<TEntity, TEntityId> _proxyRepository = proxyRepository;

    public async Task<IResult<DataTransferObjectResponse>> Handle(GenericByIdQuery<TEntity, TEntityId> query, CancellationToken cancellationToken)
    {
        var entityDto = await _proxyRepository
            .QueryByIdAsync(query.EntityId, cancellationToken, query.Mapping);

        return entityDto
            .ToResponse()
            .ToResult();
    }
}
