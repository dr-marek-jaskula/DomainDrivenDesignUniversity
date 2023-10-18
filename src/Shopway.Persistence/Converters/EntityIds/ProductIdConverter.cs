using Shopway.Domain.EntityIds;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class ProductIdConverter : ValueConverter<ProductId, string>
{
    public ProductIdConverter() : base(id => id.Value.ToString(), ulid => ProductId.Create(Ulid.Parse(ulid))) { }
}

public sealed class ProductIdComparer : ValueComparer<ProductId>
{
    public ProductIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}