using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IDynamicMapping : IMapping
{
    IList<MappingEntry> MappingEntries { get; init; }
    static abstract IReadOnlyCollection<string> AllowedProperties { get; }
}

public interface IFlatDynamicMapping<TEntity, TOutput> : IMapping<TEntity, TOutput>, IDynamicMapping
    where TEntity : class, IEntity
    where TOutput : IDictionary<string, string>
{
}

public interface IDynamicMapping<TEntity, TOutput> : IMapping<TEntity, TOutput>, IDynamicMapping
    where TEntity : class, IEntity
    where TOutput : IDictionary<string, object>
{
}