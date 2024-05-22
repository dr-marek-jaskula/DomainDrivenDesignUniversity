using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.Users;

namespace Shopway.Persistence.Converters.EntityIds;

//NOTE: LEFT FOR TUTORIAL PURPOSES. Other EntityId converters and comparers are source generated.

public sealed class UserIdConverter : ValueConverter<UserId, string>
{
    public UserIdConverter() : base(id => id.Value.ToString(), ulid => UserId.Create(Ulid.Parse(ulid))) { }
}

public sealed class UserIdComparer : ValueComparer<UserId>
{
    public UserIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}