using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Utilities.CollectionUtilities;

namespace Shopway.Persistence.Constants;

public static class SpecificationConstants
{
    public static IReadOnlyCollection<string> AllowedProductSortProperties = AsReadOnlyCollection
    (
         nameof(Product.ProductName),
         nameof(Product.Revision),
         nameof(Product.Price),
         nameof(Product.UomCode)
    );

    public static IReadOnlyCollection<string> CommonAllowedProductSortProperties = AsReadOnlyCollection
    (
         nameof(Product.ProductName),
         nameof(Product.Revision)
    );

    public static IList<SortByEntry> CommonProductSortProperties = AsList
    (
        new SortByEntry() { PropertyName = nameof(Product.ProductName), SortDirection = SortDirection.Ascending, SortPriority = 1 },
        new SortByEntry() { PropertyName = nameof(Product.Revision), SortDirection = SortDirection.Ascending, SortPriority = 2 }
    );
}