using Shopway.Domain.Common;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.GetProductsDictionary;

internal sealed class ProductDictionaryPageQueryHandler : IPageQueryHandler<ProductDictionaryPageQuery, DictionaryResponseEntry, ProductDictionaryStaticFilter, ProductDynamicSortBy, Page>
{
    private readonly IProductRepository _productRepository;

    public ProductDictionaryPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<PageResponse<DictionaryResponseEntry>>> Handle(ProductDictionaryPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, staticFilter: pageQuery.Filter, dynamicSort: pageQuery.SortBy, mapping: ProductMapping.DictionaryResponseEntry);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
