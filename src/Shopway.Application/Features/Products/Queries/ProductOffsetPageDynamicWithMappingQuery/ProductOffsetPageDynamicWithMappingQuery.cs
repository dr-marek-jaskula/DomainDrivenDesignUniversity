using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.DynamicOffsetProductWithMappingQuery;

public sealed record ProductOffsetPageDynamicWithMappingQuery(OffsetPage Page) : IOffsetPageQuery<DataTransferObjectResponse, ProductDynamicFilter, ProductDynamicSortBy, ProductDynamicMapping, OffsetPage>
{
    public ProductDynamicFilter? Filter { get; init; }
    public ProductDynamicSortBy? SortBy { get; init; }
    public ProductDynamicMapping? Mapping { get; init; }
}
