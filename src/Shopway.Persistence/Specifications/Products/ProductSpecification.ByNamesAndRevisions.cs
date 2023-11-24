﻿using Shopway.Domain.Products;
using Shopway.Persistence.Abstractions;
using static Shopway.Persistence.Constants.Constants.Specification.Product;

namespace Shopway.Persistence.Specifications.Products;

internal abstract partial class ProductSpecification
{
    internal partial class ByNamesAndRevisions : SpecificationBase<Product, ProductId>
    {
        private ByNamesAndRevisions() : base()
        {
        }

        internal static SpecificationBase<Product, ProductId> Create(IList<string> productNames, IList<string> productRevisions)
        {
            return new ByNamesAndRevisions()
                .AddFilters
                (
                    product => productNames.Contains((string)(object)product.ProductName),
                    product => productRevisions.Contains((string)(object)product.Revision)
                )
                .AddTag(QueryProductByProductNamesAndProductRevisions);
        }
    }
}