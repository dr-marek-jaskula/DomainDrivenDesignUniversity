using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class ReviewIdConverter : ValueConverter<ReviewId, Guid>
{
    public ReviewIdConverter() : base(id => id.Value, guid => ReviewId.Create(guid)) { }
}