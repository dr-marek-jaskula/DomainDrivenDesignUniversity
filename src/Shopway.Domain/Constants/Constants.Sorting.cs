using Shopway.Domain.Enums;
using Shopway.Domain.Common;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Utilities.CollectionUtilities;

namespace Shopway.Domain.Constants; 

public static partial class Constants
{
    public static partial class Sorting
    {
        public static class Product
        {
            public readonly static IReadOnlyCollection<string> AllowedProductSortProperties = AsReadOnlyCollection
            (
                 nameof(Shopway.Domain.Entities.Product.ProductName),
                 nameof(Shopway.Domain.Entities.Product.Revision),
                 nameof(Shopway.Domain.Entities.Product.Price),
                 nameof(Shopway.Domain.Entities.Product.UomCode)
            );

            public readonly static IReadOnlyCollection<string> CommonAllowedProductSortProperties = AsReadOnlyCollection
            (
                 nameof(Shopway.Domain.Entities.Product.ProductName),
                 nameof(Shopway.Domain.Entities.Product.Revision)
            );

            public readonly static IList<SortByEntry> CommonProductSortProperties = AsList
            (
                new SortByEntry() { PropertyName = nameof(Shopway.Domain.Entities.Product.ProductName), SortDirection = SortDirection.Ascending, SortPriority = 1 },
                new SortByEntry() { PropertyName = nameof(Shopway.Domain.Entities.Product.Revision), SortDirection = SortDirection.Ascending, SortPriority = 2 }
            );
        }
    }
}