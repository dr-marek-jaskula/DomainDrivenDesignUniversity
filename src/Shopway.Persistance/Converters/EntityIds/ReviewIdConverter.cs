using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class ReviewIdConverter : ValueConverter<ReviewId, Guid>
{
    public ReviewIdConverter() : base(id => id.Value, guid => ReviewId.Create(guid)) { }
}

public sealed class ReviewIdComparer : ValueComparer<ReviewId>
{
    public ReviewIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}