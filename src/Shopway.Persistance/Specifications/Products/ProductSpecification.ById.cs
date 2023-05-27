using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal sealed partial class ById : SpecificationBase<Product, ProductId>
    {
        private ById() { }

        internal static SpecificationBase<Product, ProductId> Create(ProductId productId)
        {
            return new ById()
                .AddFilters(product => product.Id == productId);
        }

        internal static SpecificationBase<Product, ProductId> Create(IList<ProductId> productIds)
        {
            return new ById()
                .AddFilters(product => productIds.Contains(product.Id));
        }

        public sealed class WithIncludes : SpecificationBase<Product, ProductId>
        {
            private WithIncludes() { }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId, params Expression<Func<Product, object>>[] includes)
            {
                return new WithIncludes()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(includes)
                    .UseSplitQuery();
            }
        }

        public sealed class WithReviews : SpecificationBase<Product, ProductId>
        {
            private WithReviews() { }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId)
            {
                return new WithReviews()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews);
            }
        }

        public sealed class WithReview : SpecificationBase<Product, ProductId>
        {
            private WithReview() { }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId, ReviewId reviewId)
            {
                return new WithReview()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews.Where(review => review.Id == reviewId));
            }

            internal static SpecificationBase<Product, ProductId> Create(ProductId productId, Title title)
            {
                return new WithReview()
                    .AddFilters(product => product.Id == productId)
                    .AddIncludes(product => product.Reviews.Where(review => review.Title.Value == title.Value));
            }
        }
    }
}