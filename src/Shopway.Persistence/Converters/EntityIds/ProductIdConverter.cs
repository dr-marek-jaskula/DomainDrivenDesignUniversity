using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class ProductIdConverter : ValueConverter<ProductId, Guid>
{
    public ProductIdConverter() : base(id => id.Value, guid => ProductId.Create(guid)) { }
}

public sealed class ProductIdComparer : ValueComparer<ProductId>
{
    public ProductIdComparer() : base((id1, id2) => id1!.Value == id2!.Value, id => id.Value.GetHashCode()) { }
}