using Shopway.Domain.Products;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class ReviewIdConverter : ValueConverter<ReviewId, string>
{
    public ReviewIdConverter() : base(id => id.Value.ToString(), ulid => ReviewId.Create(Ulid.Parse(ulid))) { }
}

public sealed class ReviewIdComparer : ValueComparer<ReviewId>
{
    public ReviewIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}