using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Converters;

public class StronglyTypedIdConverter<TEntityId> : ValueConverter<TEntityId, Guid>
    where TEntityId : IEntityId, new()
{
    public StronglyTypedIdConverter()
        : base(
            v => v.Value,
            v => new TEntityId() { Value = v })
    {
    }
}