using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IDynamicSortBy : ISortBy
{
    IList<SortByEntry> SortProperties { get; init; }
    static abstract IReadOnlyCollection<string> AllowedSortProperties { get; }
}

public interface IDynamicSortBy<TEntity> : ISortBy<TEntity>, IDynamicSortBy
    where TEntity : class, IEntity
{
}