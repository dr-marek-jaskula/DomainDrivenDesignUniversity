using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Abstractions;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class CustomerIdConverter : ValueConverter<CustomerId, Guid>
{
    public CustomerIdConverter() : base(id => id.Value, guid => CustomerId.Create(guid)) { }
}

public sealed class CustomerIdComparer : ValueComparer<CustomerId>
{
    public CustomerIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}
