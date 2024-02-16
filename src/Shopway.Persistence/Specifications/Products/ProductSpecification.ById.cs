using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;
using System.Linq.Expressions;
using static Shopway.Persistence.Constants.Constants.Specification.Product;

namespace Shopway.Persistence.Specifications.Products;

internal static partial class ProductSpecification
{
    internal static partial class ById
    {
        internal static Specification<Product, ProductId> Create(ProductId productId)
        {
            return Specification<Product, ProductId>.New()
                .AddFilters(product => product.Id == productId)
                .AddTag(QueryProductById);
        }

        internal static SpecificationWithMapping<Product, ProductId, ProductId> Create<TResponse>(IList<ProductId> productIds, Expression<Func<Product, TResponse>> mapping)
        {
            return SpecificationWithMapping<Product, ProductId, TResponse>.New()
                .AddMapping(mapping)
                .AddFilters(product => productIds.Contains(product.Id))
                .AddTag(QueryProductByIds)
                .AsMappingSpecification<ProductId>();
        }

        internal static Specification<Product, ProductId> Create(IList<ProductId> productIds)
        {
            return Specification<Product, ProductId>.New()
                .AddFilters(product => productIds.Contains(product.Id))
                .AddTag(QueryProductByIds);
        }

        public static class WithIncludes
        {
            internal static Specification<Product, ProductId> Create(ProductId productId, params Expression<Func<Product, object>>[] includes)
            {
                return Specification<Product, ProductId>.New()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(includes)
                    .UseSplitQuery()
                    .AddTag(QueryProductByIdWithIncludes);
            }
        }

        public static class WithReviews
        {
            internal static Specification<Product, ProductId> Create(ProductId productId)
            {
                return Specification<Product, ProductId>.New()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews)
                    .AddTag(QueryProductByIdWithReviews);
            }
        }

        public static class WithReview
        {
            internal static Specification<Product, ProductId> Create(ProductId productId, ReviewId reviewId)
            {
                return Specification<Product, ProductId>.New()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews.Where(review => review.Id == reviewId))
                    .AddTag(QueryProductByIdWithReviewById);
            }

            internal static Specification<Product, ProductId> Create(ProductId productId, Title title)
            {
                return Specification<Product, ProductId>.New()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews.Where(review => review.Title == title))
                    .AddTag(QueryProductByIdWithReviewByTitle);
            }
        }
    }
}