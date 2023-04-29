using Shopway.Domain.Common;

namespace Shopway.Domain.Abstractions;

public interface ISortBy
{
    IList<SortByEntry> SortProperties { get; init; }
    IReadOnlyCollection<string> AllowedSortProperties { get; init; }
}

public interface ISortBy<TEntity> : ISortBy
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}