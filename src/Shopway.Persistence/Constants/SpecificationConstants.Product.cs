using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using System.Linq.Expressions;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Utilities.CollectionUtilities;
using static Shopway.Domain.Utilities.EnumUtilities;

namespace Shopway.Persistence.Constants;

public static class SpecificationConstants
{
    public static string QueryProductById = "Query product by id";
    public static string QueryProductByKey = "Query product by key";
    public static string QueryProductByIdWithIncludes = "Query product by id with includes";
    public static string QueryProductByIdWithReviews = "Query product by id with reviews";
    public static string QueryProductByIdWithReviewById = "Query product by id with review by id";
    public static string QueryProductByIdWithReviewByTitle = "Query product by id with review by title";
    public static string QueryProductByIds = "Query product by ids";
    public static string QueryProductByProductNamesAndProductRevisions = "Query product by names and revisions";

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

    public static IReadOnlyCollection<string> AllowedProductFilterProperties = AsReadOnlyCollection
    (
         nameof(Product.ProductName),
         nameof(Product.Revision),
         nameof(Product.Price),
         nameof(Product.UomCode)
    );

    public static IReadOnlyCollection<string> AllowedProductFilterOperations = AsList
    (
         nameof(string.Contains)
    )
        .Concat(GetNamesOf<ExpressionType>())
        .ToList()
        .AsReadOnly();
}