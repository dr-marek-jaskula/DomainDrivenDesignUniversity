using Shopway.Domain.Common;

namespace Shopway.Domain.Abstractions;

public interface IExpressionFilter : IFilter
{
    IList<FilterByEntry> FilterProperties { get; init; }
    IReadOnlyCollection<string> AllowedFilterProperties { get; init; }
}

public interface IExpressionFilter<TEntity> : IExpressionFilter
    where TEntity : class, IEntity
{
    abstract IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}