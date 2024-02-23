using static Shopway.Domain.Common.Utilities.CollectionUtilities;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static partial class Mapping
    {
        public static class Product
        {
            public readonly static IReadOnlyCollection<string> AllowedProductMappingProperties = AsReadOnlyCollection
            (
                 nameof(Products.Product.ProductName),
                 nameof(Products.Product.Revision),
                 nameof(Products.Product.Price),
                 nameof(Products.Product.UomCode)
            );
        }
    }
}