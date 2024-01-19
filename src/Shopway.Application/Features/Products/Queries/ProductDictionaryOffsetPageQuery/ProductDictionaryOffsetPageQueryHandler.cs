﻿using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;

internal sealed class ProductDictionaryOffsetPageQueryHandler(IProductRepository productRepository)
    : IOffsetPageQueryHandler<ProductDictionaryOffsetPageQuery, DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, OffsetPage>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<OffsetPageResponse<DictionaryResponseEntry>>> Handle(ProductDictionaryOffsetPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: ProductMapping.DictionaryResponseEntry);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
