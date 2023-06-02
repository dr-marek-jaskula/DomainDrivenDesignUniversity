using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class CustomerIdConverter : ValueConverter<CustomerId, Guid>
{
    public CustomerIdConverter() : base(id => id.Value, guid => CustomerId.Create(guid)) { }
}