using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.GenericPageQuery;

public sealed record GenericCursorPageQuery<TEntity, TEntityId>(CursorPage Page) : ICursorPageQuery<DataTransferObjectResponse, DynamicFilter<TEntity, TEntityId>, DynamicSortBy<TEntity, TEntityId>, DynamicMapping<TEntity, TEntityId>, CursorPage>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public DynamicFilter<TEntity, TEntityId>? Filter { get; init; }
    public DynamicSortBy<TEntity, TEntityId>? SortBy { get; init; }
    public DynamicMapping<TEntity, TEntityId>? Mapping { get; init; }

    public static GenericCursorPageQuery<TEntity, TEntityId> From(GenericProxyPageQuery proxyQuery)
    {
        var filter = DynamicFilter<TEntity, TEntityId>.From(proxyQuery.Filter);
        var sortBy = DynamicSortBy<TEntity, TEntityId>.From(proxyQuery.SortBy);
        var mapping = DynamicMapping<TEntity, TEntityId>.From(proxyQuery.Mapping);

        bool noMappingForCursorWhenMappingIsNotNull = mapping is not null && mapping
            .MappingEntries
            .Any(x => x.PropertyName is nameof(IEntityId.Id)) is false;

        if (noMappingForCursorWhenMappingIsNotNull)
        {
            mapping!.MappingEntries.Insert(0, new MappingEntry()
            {
                PropertyName = nameof(IEntityId.Id)
            });
        }

        return new GenericCursorPageQuery<TEntity, TEntityId>(proxyQuery.Page.ToCursorPage())
        {
            Filter = filter,
            SortBy = sortBy,
            Mapping = mapping,
        };
    }
}
