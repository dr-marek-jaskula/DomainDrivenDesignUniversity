using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.Features.Products.Queries.GetProductsCursorDictionary;

internal sealed class ProductDictionaryCursorPageQueryHandler : ICursorPageQueryHandler<ProductDictionaryCursorPageQuery, DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, CursorPage>
{
    private readonly IProductRepository _productRepository;

    public ProductDictionaryCursorPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<CursorPageResponse<DictionaryResponseEntry>>> Handle(ProductDictionaryCursorPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: ProductMapping.DictionaryResponseEntry);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
