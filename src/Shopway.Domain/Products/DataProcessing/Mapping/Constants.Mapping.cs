using Shopway.Domain.Products;
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
                 nameof(Products.Product.Id),
                 nameof(Products.Product.ProductName),
                 nameof(Products.Product.Revision),
                 nameof(Products.Product.Price),
                 nameof(Products.Product.UomCode),
                 nameof(Products.Product.Reviews),
                 $"{nameof(Products.Product.Reviews)}.{nameof(Review.Id)}",
                 $"{nameof(Products.Product.Reviews)}.{nameof(Review.Description)}",
                 $"{nameof(Products.Product.Reviews)}.{nameof(Review.Title)}",
                 $"{nameof(Products.Product.Reviews)}.{nameof(Review.Username)}",
                 $"{nameof(Products.Product.Reviews)}.{nameof(Review.Stars)}"
            );
        }
    }
}
