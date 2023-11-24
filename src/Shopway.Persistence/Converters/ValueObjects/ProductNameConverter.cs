using Shopway.Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class ProductNameConverter : ValueConverter<ProductName, string>
{
    public ProductNameConverter() : base(productName => productName.Value, @string => ProductName.Create(@string).Value) { }
}

public sealed class ProductNameComparer : ValueComparer<ProductName>
{
    public ProductNameComparer() : base((productName1, productName2) => productName1!.Value == productName2!.Value, productName => productName.GetHashCode()) { }
}