using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class UserIdConverter : ValueConverter<UserId, Guid>
{
    public UserIdConverter() : base(id => id.Value, guid => UserId.Create(guid)) { }
}
