using Shopway.Domain.Common;

namespace Shopway.Domain.Abstractions.Common;

public interface IDynamicFilter : IFilter
{
    IList<FilterByEntry> FilterProperties { get; init; }
    abstract static IReadOnlyCollection<string> AllowedFilterProperties { get; }
    abstract static IReadOnlyCollection<string> AllowedFilterOperations { get; }
}

public interface IDynamicFilter<TEntity> : IFilter<TEntity>, IDynamicFilter
    where TEntity : class, IEntity
{
}