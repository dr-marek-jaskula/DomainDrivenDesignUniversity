using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Abstractions;
using System.Linq.Expressions;
using static Shopway.Persistence.Constants.SpecificationConstants;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal sealed partial class ById : SpecificationBase<Product, ProductId>
    {
        private ById() { }

        internal static SpecificationBase<Product, ProductId> Create(ProductId productId)
        {
            return new ById()
                .AddFilters(product => product.Id == productId)
                .AddTag(QueryProductById);
        }

        internal static SpecificationBase<Product, ProductId> Create(IList<ProductId> productIds)
        {
            return new ById()
                .AddFilters(product => productIds.Contains(product.Id))
                .AddTag(QueryProductByIds);
        }

        public sealed class WithIncludes : SpecificationBase<Product, ProductId>
        {
            private WithIncludes() { }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId, params Expression<Func<Product, object>>[] includes)
            {
                return new WithIncludes()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(includes)
                    .UseSplitQuery()
                    .AddTag(QueryProductByIdWithIncludes);
            }
        }

        public sealed class WithReviews : SpecificationBase<Product, ProductId>
        {
            private WithReviews() { }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId)
            {
                return new WithReviews()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews)
                    .AddTag(QueryProductByIdWithReviews);
            }
        }

        public sealed class WithReview : SpecificationBase<Product, ProductId>
        {
            private WithReview() { }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId, ReviewId reviewId)
            {
                return new WithReview()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews.Where(review => review.Id == reviewId))
                    .AddTag(QueryProductByIdWithReviewById);
            }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId, Title title)
            {
                return new WithReview()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews.Where(review => review.Title == title))
                    .AddTag(QueryProductByIdWithReviewByTitle);
            }
        }
    }
}