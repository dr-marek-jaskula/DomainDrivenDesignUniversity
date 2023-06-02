using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Converters.EntityIds;

public sealed class ProductIdConverter : ValueConverter<ProductId, Guid>
{
    public ProductIdConverter() : base(id => id.Value, guid => ProductId.Create(guid)) { }
}