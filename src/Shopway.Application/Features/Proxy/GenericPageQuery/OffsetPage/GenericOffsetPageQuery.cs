using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy;

public sealed record GenericOffsetPageQuery<TEntity, TEntityId>(OffsetPage Page) : IOffsetPageQuery<DataTransferObjectResponse, DynamicFilter<TEntity, TEntityId>, DynamicSortBy<TEntity, TEntityId>, DynamicMapping<TEntity, TEntityId>, OffsetPage>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public DynamicFilter<TEntity, TEntityId>? Filter { get; init; }
    public DynamicSortBy<TEntity, TEntityId>? SortBy { get; init; }
    public DynamicMapping<TEntity, TEntityId>? Mapping { get; init; }

    public static GenericOffsetPageQuery<TEntity, TEntityId> From(GenericProxyPageQuery proxyQuery)
    {
        var filter = DynamicFilter<TEntity, TEntityId>.From(proxyQuery.Filter);
        var sortBy = DynamicSortBy<TEntity, TEntityId>.From(proxyQuery.SortBy);
        var mapping = DynamicMapping<TEntity, TEntityId>.From(proxyQuery.Mapping);

        return new GenericOffsetPageQuery<TEntity, TEntityId>(proxyQuery.Page.ToOffsetPage())
        {
            Filter = filter,
            SortBy = sortBy,
            Mapping = mapping,
        };
    }
}
