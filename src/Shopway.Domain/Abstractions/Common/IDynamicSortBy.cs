using Shopway.Domain.Common;

namespace Shopway.Domain.Abstractions.Common;

public interface IDynamicSortBy : ISortBy
{
    IList<SortByEntry> SortProperties { get; init; }
    IReadOnlyCollection<string> AllowedSortProperties { get; init; }
}

public interface IDynamicSortBy<TEntity> : IDynamicSortBy
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}