namespace Shopway.Persistence.Constants;

public static partial class Constants
{
    public static partial class Specification
    {
        public static class Product
        {
            public const string QueryProductById = "Query product by id";
            public const string QueryProductByKey = "Query product by key";
            public const string QueryProductByIdWithIncludes = "Query product by id with includes";
            public const string QueryProductByIdWithReviews = "Query product by id with reviews";
            public const string QueryProductByIdWithReviewById = "Query product by id with review by id";
            public const string QueryProductByIdWithReviewByTitle = "Query product by id with review by title";
            public const string QueryProductByIds = "Query product by ids";
            public const string QueryProductByProductNamesAndProductRevisions = "Query product by names and revisions";
            public const string QueryProductNames = "Query product names";
        }
    }
}