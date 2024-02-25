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
                 nameof(Products.Product.UomCode),
                 nameof(Products.Product.Reviews),
                 nameof(Products.Review.Description),
                 nameof(Products.Review.Stars),
                 nameof(Products.Review.Title),
                 nameof(Products.Review.Username)
            );
        }
    }
}