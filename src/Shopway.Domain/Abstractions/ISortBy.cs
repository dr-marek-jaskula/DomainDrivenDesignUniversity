using Shopway.Domain.Helpers;
using System.Collections.ObjectModel;

namespace Shopway.Domain.Abstractions;

public interface ISortBy
{
    IList<OrderEntry> SortProperties { get; init; }
    ReadOnlyCollection<string> AllowedSortProperties { get; init; }
}

public interface ISortBy<TEntity> : ISortBy
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}