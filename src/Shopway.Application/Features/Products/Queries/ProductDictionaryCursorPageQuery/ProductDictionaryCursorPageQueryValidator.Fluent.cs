﻿using Shopway.Domain.Common;
using Shopway.Application.Sorting.Products;
using Shopway.Application.Filering.Products;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;

internal sealed class ProductDictionaryCursorPageQueryValidator : CursorPageQueryValidator<ProductDictionaryCursorPageQuery, DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, CursorPage>
{
    public ProductDictionaryCursorPageQueryValidator()
        : base()
    {
    }
}