using Shopway.Domain.Common;

namespace Shopway.Domain.Abstractions.Common;

public interface IDynamicFilter : IFilter
{
    IList<FilterByEntry> FilterProperties { get; init; }
    IReadOnlyCollection<string> AllowedFilterProperties { get; }
    IReadOnlyCollection<string> AllowedFilterOperations { get; }
}

public interface IDynamicFilter<TEntity> : IDynamicFilter
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}