using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public sealed class DynamicSortBy : IDynamicSortBy
{
    public static IReadOnlyCollection<string> AllowedSortProperties => [];

    public IList<SortByEntry> SortProperties { get; init; } = [];

    public TDynamicSortBy To<TDynamicSortBy, TEntity>()
        where TDynamicSortBy : class, IDynamicSortBy<TEntity>, new()
        where TEntity : class, IEntity
    {
        return new TDynamicSortBy()
        {
            SortProperties = SortProperties
        };
    }
}

public sealed class DynamicSortBy<TEntity, TEntityId> : IDynamicSortBy<TEntity>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public static IReadOnlyCollection<string> AllowedSortProperties => [];

    public IList<SortByEntry> SortProperties { get; init; } = [];

    public IQueryable<TEntity> Apply(IQueryable<TEntity> queryable)
    {
        return queryable.Sort(SortProperties);
    }

    public static DynamicSortBy<TEntity, TEntityId>? From(DynamicSortBy? dynamicSortBy)
    {
        if (dynamicSortBy is null)
        {
            return null;
        }

        return new DynamicSortBy<TEntity, TEntityId>()
        {
            SortProperties = dynamicSortBy.SortProperties
        };
    }
}
