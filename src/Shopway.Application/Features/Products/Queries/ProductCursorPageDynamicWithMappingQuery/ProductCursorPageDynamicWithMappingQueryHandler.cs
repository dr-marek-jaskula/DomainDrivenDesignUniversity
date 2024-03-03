using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.ProductCursorPageDynamicWithMappingQuery;

internal sealed class ProductCursorPageDynamicWithMappingQueryHandler(IProductRepository productRepository)
    : ICursorPageQueryHandler<ProductCursorPageDynamicWithMappingQuery, DataTransferObjectResponse, ProductDynamicFilter, ProductDynamicSortBy, ProductDynamicMapping, CursorPage>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<CursorPageResponse<DataTransferObjectResponse>>> Handle(ProductCursorPageDynamicWithMappingQuery pageQuery, CancellationToken cancellationToken)
    {
        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, filter: pageQuery.Filter, sort: pageQuery.SortBy, mapping: pageQuery.Mapping);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
