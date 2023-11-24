using static Shopway.Domain.Common.Utilities.ListUtilities;
using static Shopway.Domain.Common.Utilities.CollectionUtilities;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static partial class Sorting
    {
        public static class Product
        {
            public readonly static IReadOnlyCollection<string> AllowedProductSortProperties = AsReadOnlyCollection
            (
                 nameof(Products.Product.ProductName),
                 nameof(Products.Product.Revision),
                 nameof(Products.Product.Price),
                 nameof(Products.Product.UomCode)
            );

            public readonly static IReadOnlyCollection<string> CommonAllowedProductSortProperties = AsReadOnlyCollection
            (
                 nameof(Products.Product.ProductName),
                 nameof(Products.Product.Revision)
            );

            public readonly static IList<SortByEntry> CommonProductSortProperties =
            [
                new SortByEntry() { PropertyName = nameof(Products.Product.ProductName), SortDirection = SortDirection.Ascending, SortPriority = 1 },
                new SortByEntry() { PropertyName = nameof(Products.Product.Revision), SortDirection = SortDirection.Ascending, SortPriority = 2 }
            ];
        }
    }
}