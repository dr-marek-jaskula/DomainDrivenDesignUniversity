using Shopway.Domain.Enums;
using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using System.Linq.Expressions;
using static Shopway.Domain.Utilities.EnumUtilities;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Utilities.CollectionUtilities;

namespace Shopway.Persistence.Constants;

public static class SpecificationConstants
{
    public const string QueryProductById = "Query product by id";
    public const string QueryProductByKey = "Query product by key";
    public const string QueryProductByIdWithIncludes = "Query product by id with includes";
    public const string QueryProductByIdWithReviews = "Query product by id with reviews";
    public const string QueryProductByIdWithReviewById = "Query product by id with review by id";
    public const string QueryProductByIdWithReviewByTitle = "Query product by id with review by title";
    public const string QueryProductByIds = "Query product by ids";
    public const string QueryProductByProductNamesAndProductRevisions = "Query product by names and revisions";

    public readonly static IReadOnlyCollection<string> AllowedProductSortProperties = AsReadOnlyCollection
    (
         nameof(Product.ProductName),
         nameof(Product.Revision),
         nameof(Product.Price),
         nameof(Product.UomCode)
    );

    public readonly static IReadOnlyCollection<string> CommonAllowedProductSortProperties = AsReadOnlyCollection
    (
         nameof(Product.ProductName),
         nameof(Product.Revision)
    );

    public readonly static IList<SortByEntry> CommonProductSortProperties = AsList
    (
        new SortByEntry() { PropertyName = nameof(Product.ProductName), SortDirection = SortDirection.Ascending, SortPriority = 1 },
        new SortByEntry() { PropertyName = nameof(Product.Revision), SortDirection = SortDirection.Ascending, SortPriority = 2 }
    );

    public readonly static IReadOnlyCollection<string> AllowedProductFilterProperties = AsReadOnlyCollection
    (
         nameof(Product.ProductName),
         nameof(Product.Revision),
         nameof(Product.Price),
         nameof(Product.UomCode)
    );

    public readonly static IReadOnlyCollection<string> AllowedProductFilterOperations = AsList
    (
         nameof(string.Contains),
         nameof(string.StartsWith),
         nameof(string.EndsWith)
    )
        .Concat(GetNamesOf<ExpressionType>())
        .ToList()
        .AsReadOnly();
}