using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public sealed class DynamicMapping : IDynamicMapping
{
    public static IReadOnlyCollection<string> AllowedProperties => [];

    public IList<MappingEntry> MappingEntries { get; init; } = [];

    public TDynamicMapping To<TDynamicMapping, TEntity>()
        where TDynamicMapping : class, IDynamicMapping<TEntity>, new()
        where TEntity : class, IEntity
    {
        return new TDynamicMapping()
        {
            MappingEntries = MappingEntries
        };
    }
}

public sealed class DynamicMapping<TEntity, TEntityId> : IDynamicMapping<TEntity>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public static IReadOnlyCollection<string> AllowedProperties => [];

    public IList<MappingEntry> MappingEntries { get; init; } = [];

    public IQueryable<DataTransferObject> Apply(IQueryable<TEntity> queryable)
    {
        return queryable
            .Map(MappingEntries);
    }

    public static DynamicMapping<TEntity, TEntityId>? From(DynamicMapping? dynamicMapping)
    {
        if (dynamicMapping is null)
        {
            return null;
        }

        return new DynamicMapping<TEntity, TEntityId>()
        {
            MappingEntries = dynamicMapping.MappingEntries
        };
    }
}
