using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IDynamicMapping : IMapping
{
    IList<MappingEntry> MappingEntries { get; init; }
    static abstract IReadOnlyCollection<string> AllowedProperties { get; }
}

public interface IDynamicMapping<TEntity> : IMapping<TEntity, DataTransferObject>, IDynamicMapping
    where TEntity : class, IEntity
{
}
