using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.ProductCursorPageDynamicWithMappingQuery;

public sealed record ProductCursorPageDynamicWithMappingQuery(CursorPage Page) : ICursorPageQuery<DataTransferObjectResponse, ProductDynamicFilter, ProductDynamicSortBy, ProductDynamicMapping, CursorPage>
{
    public ProductDynamicFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
    public ProductDynamicMapping? Mapping { get; init; }
}
