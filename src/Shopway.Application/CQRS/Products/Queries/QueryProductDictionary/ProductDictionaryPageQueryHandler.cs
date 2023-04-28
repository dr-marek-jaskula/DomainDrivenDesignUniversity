using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Specifications.Products;
using Shopway.Application.Utilities;
using Shopway.Application.Mappings;

namespace Shopway.Application.CQRS.Products.Queries.GetProductsDictionary;

internal sealed class ProductDictionaryPageQueryHandler : IPageQueryHandler<ProductDictionaryPageQuery, DictionaryResponseEntry, ProductDictionaryFilter, ProductOrder, Page>
{
    private readonly IProductRepository _productRepository;

    public ProductDictionaryPageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<PageResponse<DictionaryResponseEntry>>> Handle(ProductDictionaryPageQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Filter, pageQuery.Order, pageQuery.Page, ProductMapping.ToDictionaryResponseEntry(), cancellationToken);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
