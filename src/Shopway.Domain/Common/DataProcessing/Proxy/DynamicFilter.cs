using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.DataProcessing.Proxy;

public sealed class DynamicFilter : IDynamicFilter
{
    public static IReadOnlyCollection<string> AllowedFilterProperties => [];

    public static IReadOnlyCollection<string> AllowedFilterOperations => [];

    public IList<FilterByEntry> FilterProperties { get; init; } = [];

    public TDynamicFilter To<TDynamicFilter, TEntity>() 
        where TDynamicFilter : class, IDynamicFilter<TEntity>, new()
        where TEntity : class, IEntity
    {
        return new TDynamicFilter()
        {
            FilterProperties = FilterProperties
        };
    }
}

public sealed class DynamicFilter<TEntity, TEntityId> : IDynamicFilter<TEntity>
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    public static IReadOnlyCollection<string> AllowedFilterProperties => [];

    public static IReadOnlyCollection<string> AllowedFilterOperations => [];

    public IList<FilterByEntry> FilterProperties { get; init; } = [];

    public IQueryable<TEntity> Apply(IQueryable<TEntity> queryable, ILikeProvider<TEntity>? likeProvider = null)
    {
        if (FilterProperties.IsNullOrEmpty())
        {
            return queryable;
        }

        return queryable
            .Where(FilterProperties.CreateFilterExpression(likeProvider));
    }

    public static DynamicFilter<TEntity, TEntityId>? From(DynamicFilter? dynamicFilter)
    {
        if (dynamicFilter is null)
        {
            return null;
        }

        return new DynamicFilter<TEntity, TEntityId>()
        {
            FilterProperties = dynamicFilter.FilterProperties
        };
    }
}
