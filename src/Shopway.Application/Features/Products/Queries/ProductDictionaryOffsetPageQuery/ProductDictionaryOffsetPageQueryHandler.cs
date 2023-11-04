﻿using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Sorting.Products;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Queries.GetProductsOffsetDictionary;

internal sealed class ProductDictionaryOffsetPageQueryHandler : IOffsetPageQueryHandler<ProductDictionaryOffsetPageQuery, DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, OffsetPage>
{
    private readonly IProductRepository _productRepository;

    public ProductDictionaryOffsetPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<OffsetPageResponse<DictionaryResponseEntry>>> Handle(ProductDictionaryOffsetPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: ProductMapping.DictionaryResponseEntry);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
