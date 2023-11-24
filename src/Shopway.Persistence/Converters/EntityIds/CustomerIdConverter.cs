using Shopway.Domain.Users;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class CustomerIdConverter : ValueConverter<CustomerId, string>
{
    public CustomerIdConverter() : base(id => id.Value.ToString(), ulid => CustomerId.Create(Ulid.Parse(ulid))) { }
}

public sealed class CustomerIdComparer : ValueComparer<CustomerId>
{
    public CustomerIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}
