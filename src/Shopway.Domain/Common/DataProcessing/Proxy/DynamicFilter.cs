using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

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