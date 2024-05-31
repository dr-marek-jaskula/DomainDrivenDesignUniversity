using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryById;

public sealed record GenericQuery<TEntity, TEntityId>(TEntityId EntityId) : IQuery<DataTransferObjectResponse>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public DynamicMapping<TEntity, TEntityId>? Mapping { get; init; }

    public static GenericQuery<TEntity, TEntityId> From(GenericProxyQuery proxyQuery)
    {
        var mapping = DynamicMapping<TEntity, TEntityId>.From(proxyQuery.Mapping);

        return new GenericQuery<TEntity, TEntityId>(TEntityId.Create(proxyQuery.Id))
        {
            Mapping = mapping,
        };
    }
}
