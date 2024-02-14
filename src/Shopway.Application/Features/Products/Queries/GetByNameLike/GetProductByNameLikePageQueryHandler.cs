using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Sorting;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Application.Features.Products.Queries.GetProductById;

internal sealed class GetProductByNameLikePageQueryHandler(IProductRepository productRepository)
    : IOffsetPageQueryHandler<GetProductByNameLikePageQuery, ProductResponse, OffsetPage>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<OffsetPageResponse<ProductResponse>>> Handle(GetProductByNameLikePageQuery pageQuery, CancellationToken cancellationToken)
    {
        //var likes = AsList(new LikeEntry<Product>(product => product.ProductName.Value, pageQuery.ProductNameLikePattern));
        var likes = AsList(new LikeEntry<Product>(product => product.CreatedBy, pageQuery.ProductNameLikePattern));

        var page = await _productRepository
            .PageAsync(pageQuery.Page, cancellationToken, likes: likes, sort: ProductSortBy.Common, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(pageQuery.Page)
            .ToResult();
    }
}
