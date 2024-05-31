using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;

public sealed record GenericByKeyQuery<TEntity, TEntityId, TEntityKey>(TEntityKey EntityKey) : IQuery<DataTransferObjectResponse>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntityKey : IUniqueKey<TEntity, TEntityKey>
{
    public DynamicMapping<TEntity, TEntityId>? Mapping { get; init; }

    public static GenericByKeyQuery<TEntity, TEntityId, TEntityKey> From(GenericProxyByKeyQuery proxyQuery)
    {
        var mapping = DynamicMapping<TEntity, TEntityId>.From(proxyQuery.Mapping);

        TEntityKey key;

        try
        {
            key = TEntityKey.Create(proxyQuery.Key);
        }
        catch (Exception exception)
        {
            throw new ArgumentException($"Invalid {typeof(TEntityKey).Name} Components. {exception.Message}");
        }

        return new GenericByKeyQuery<TEntity, TEntityId, TEntityKey>(key)
        {
            Mapping = mapping,
        };
    }
}
